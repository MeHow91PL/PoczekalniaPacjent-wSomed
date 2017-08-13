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
        public void initApp()
        {
            WyswietlaczManager wysManager = new WyswietlaczManager();
            this.AddedNewNumber += wysManager.AddedNewNumber;
            this.RemovedNumber += wysManager.RemovedNumber;
            //this.ChangedDataBase += wysManager.ChangrdDataBase;
            Timer timer1 = new Timer(1000);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }


        static List<ProszonyPacjentModel> listaWezwanych = new List<ProszonyPacjentModel>();
        static Queue<ProszonyPacjentModel> kolejkaWezwań = new Queue<ProszonyPacjentModel>();
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();

        public delegate void ChangedDbEventHandler(object o, ChangedDbEventArgs e);
        public event ChangedDbEventHandler AddedNewNumber;
        public event ChangedDbEventHandler RemovedNumber;
        //public event ChangedDbEventHandler ChangedDataBase;

        protected virtual void OnAddedNewNumber(ProszonyPacjentModel ZmienionyRecord, List<ProszonyPacjentModel> listaWezwanych)
        {
            if (AddedNewNumber != null)
            {
                AddedNewNumber(this, new ChangedDbEventArgs
                {
                    ZmienionyRecord = ZmienionyRecord,
                    AktualnieWzywani = listaWezwanych
                });
            }
        }

        protected virtual void OnRemovedNumber(ProszonyPacjentModel ZmienionyRecord, List<ProszonyPacjentModel> listaWezwanych)
        {
            if (RemovedNumber != null)
            {
                RemovedNumber(this, new ChangedDbEventArgs
                {
                    ZmienionyRecord = ZmienionyRecord,
                    AktualnieWzywani = listaWezwanych
                });
            }
        }

        //protected virtual void OnChangedDataBase(List<ProszonyPacjentModel> aktualnaLista)
        //{
        //    ChangedDataBase?.Invoke(this, new ChangedDbEventArgs { AktualnieWzywani = aktualnaLista });
        //}

        OracleDbManager db = new OracleDbManager();

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {

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