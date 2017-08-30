using Microsoft.AspNet.SignalR;
using Poczekalniav1.DAL;
using Poczekalniav1.Hubs;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Poczekalniav1.Infrastructure
{
    public class NasluchBazy
    {
        static List<ProszonyPacjentModel> listaWezwanych = new List<ProszonyPacjentModel>();
        static Queue<ProszonyPacjentModel> kolejkaWezwań = new Queue<ProszonyPacjentModel>();
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
        DbManager db = new OracleDbManager();

        public void initApp()
        {
            WyswietlaczManager wysManager = new WyswietlaczManager();
            this.AddedNewNumber += wysManager.AddedNewNumber;
            this.RemovedNumber += wysManager.RemovedNumber;
            this.DisconnectedFromDb += wysManager.ShowDbSettings;
            this.DbConnectionResumed += wysManager.DbConnResumed;

            Timer timer1 = new Timer(5000);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }
        #region Events
        public delegate void ChangedDbEventHandler(object o, ChangedDbEventArgs e);
        public event ChangedDbEventHandler AddedNewNumber;
        protected virtual void OnAddedNewNumber(ProszonyPacjentModel ZmienionyRecord, List<ProszonyPacjentModel> listaWezwanych)
        {
            AddedNewNumber?.Invoke(this, new ChangedDbEventArgs
            {
                ZmienionyRecord = ZmienionyRecord,
                AktualnieWzywani = listaWezwanych
            });
        }
        public event ChangedDbEventHandler RemovedNumber;
        protected virtual void OnRemovedNumber(ProszonyPacjentModel ZmienionyRecord, List<ProszonyPacjentModel> listaWezwanych)
        {
            RemovedNumber?.Invoke(this, new ChangedDbEventArgs
            {
                ZmienionyRecord = ZmienionyRecord,
                AktualnieWzywani = listaWezwanych
            });
        }

        public delegate void DisconnectedFromDbEventHandler(object o, EventArgs e);
        public event DisconnectedFromDbEventHandler DisconnectedFromDb;
        protected virtual void OnDisconnectedFromDb()
        {
            DisconnectedFromDb?.Invoke(this, EventArgs.Empty);
        }

        public delegate void DbConnectionResumedEventHandler(object o, EventArgs e);
        public event DbConnectionResumedEventHandler DbConnectionResumed;
        protected virtual void OnDbConnectionResumed()
        {
            DbConnectionResumed?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        bool utraconoPolaczenie = false;
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!db.CzyPolaczonoPoprawnie())
            {
                if (utraconoPolaczenie == false)
                {
                    OnDisconnectedFromDb();
                }
                utraconoPolaczenie = true;
            }
            else
            {
                if (utraconoPolaczenie)
                {
                    OnDbConnectionResumed();
                    utraconoPolaczenie = false;
                }

                if (db.IloscProszonychPacjentow > 0 || listaWezwanych.Count > 0)
                {
                    foreach (var item in db.PobierzProszonychPacjentow())
                    {
                        if (!listaWezwanych.Any(l => l.NUMER_DZIENNY == item.NUMER_DZIENNY))
                        {
                            listaWezwanych.Add(item);
                            OnAddedNewNumber(item, db.PobierzProszonychPacjentow());
                        }
                    }

                    foreach (var item in listaWezwanych)
                    {
                        if (!db.PobierzProszonychPacjentow().Any(l => l.NUMER_DZIENNY == item.NUMER_DZIENNY))
                        {
                            OnRemovedNumber(item, db.PobierzProszonychPacjentow());
                        }
                    }
                    listaWezwanych = db.PobierzProszonychPacjentow().FindAll(w => listaWezwanych.Exists(l => l.NUMER_DZIENNY == w.NUMER_DZIENNY));
                }
            }
        }
    }
}