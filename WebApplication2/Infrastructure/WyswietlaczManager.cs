using Microsoft.AspNet.SignalR;
using Poczekalniav1.Hubs;
using Poczekalniav1.Models;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Poczekalniav1.Infrastructure
{
    public class WyswietlaczManager
    {
        private ProszonyPacjentModel _aktualnieWyswietlanyNumer;
        private List<ProszonyPacjentModel> _ListaDoWyswietlenia = new List<ProszonyPacjentModel>();
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();


        public WyswietlaczManager()
        {
            Timer t1 = new Timer(1000);
            t1.Elapsed += T1_Elapsed;
            t1.Start();
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_aktualnieWyswietlanyNumer == null && _ListaDoWyswietlenia.Count > 0)
            {
                _aktualnieWyswietlanyNumer = _ListaDoWyswietlenia[0];
                _ListaDoWyswietlenia.Remove(_aktualnieWyswietlanyNumer);
                context.Clients.All.wyswietlNr(_aktualnieWyswietlanyNumer);
                System.Threading.Thread.Sleep(10000);
                context.Clients.All.ukryjNr(_aktualnieWyswietlanyNumer);
                _aktualnieWyswietlanyNumer = null;
            }
        }

        public void AddedNewNumber(object o, ChangedDbEventArgs e)
        {
            if (!_ListaDoWyswietlenia.Contains(e.ZmienionyRecord))
            {
                _ListaDoWyswietlenia.Add(e.ZmienionyRecord);
            }
        }

        public void RemovedNumber(object o, ChangedDbEventArgs e)
        {
            if (_ListaDoWyswietlenia.Contains(e.ZmienionyRecord))
            {
                _ListaDoWyswietlenia.Remove(e.ZmienionyRecord);
            }
        }
    }
}