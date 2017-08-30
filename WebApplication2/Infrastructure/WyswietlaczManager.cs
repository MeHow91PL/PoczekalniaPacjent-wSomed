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
        private static List<ProszonyPacjentModel> _ListaDoWyswietlenia = new List<ProszonyPacjentModel>();
        private List<ProszonyPacjentModel> _listaWezwanychPacjentow = new List<ProszonyPacjentModel>();
        static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();


        public WyswietlaczManager()
        {
            Timer t1 = new Timer();
            t1.Interval = 2000;
            t1.Elapsed += T1_Elapsed;
            t1.Start();
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_aktualnieWyswietlanyNumer == null && _ListaDoWyswietlenia.Count > 0)
            {
                _aktualnieWyswietlanyNumer = _ListaDoWyswietlenia[0];
                context.Clients.All.wyswietlNr(_aktualnieWyswietlanyNumer, "Wyświetlony: " + e.SignalTime.Millisecond);
                System.Threading.Thread.Sleep(OpcjeAplikacjiManager.WzywanyNumer.CzasWyswietlania * 1000);
                context.Clients.All.aktuKolejkeDoWezwania(_listaWezwanychPacjentow, gabinety(_listaWezwanychPacjentow));
                context.Clients.All.ukryjNr(_aktualnieWyswietlanyNumer, "Ukryty: " + e.SignalTime.Millisecond);
                _ListaDoWyswietlenia.Remove(_aktualnieWyswietlanyNumer);
                _aktualnieWyswietlanyNumer = null;
            }
        }

        public void AddedNewNumber(object o, ChangedDbEventArgs e)
        {
            if (!_ListaDoWyswietlenia.Contains(e.ZmienionyRecord))
            {
                _ListaDoWyswietlenia.Add(e.ZmienionyRecord);
            }
            _listaWezwanychPacjentow = e.AktualnieWzywani;
        }

        public void RemovedNumber(object o, ChangedDbEventArgs e)
        {
            if (_ListaDoWyswietlenia.Contains(e.ZmienionyRecord))
            {
                _ListaDoWyswietlenia.Remove(e.ZmienionyRecord);
            }
            context.Clients.All.aktuKolejkeDoWezwania(e.AktualnieWzywani, gabinety(e.AktualnieWzywani));
        }

        public void ShowDbSettings(object o, EventArgs e)
        {
            context.Clients.All.addLogMessage("Utracono połączenie z bazą danych. Czas: " + DateTime.Now);
        }

        public void DbConnResumed(object o, EventArgs e)
        {
            context.Clients.All.addLogMessage("Wznowiono połączenie z bazą danych. Czas: " + DateTime.Now);
        }

        internal static void RefreshNumberList(string clientId, List<ProszonyPacjentModel> AktualnieWzywani)
        {
            context.Clients.Client(clientId).aktuKolejkeDoWezwania(AktualnieWzywani, gabinety(AktualnieWzywani));
        }
        //public void ChangrdDataBase(object o, ChangedDbEventArgs e)
        //{
        //    _ListaPacjentowWGabinecie = e.AktualnieWzywani;
        //}

        public static List<Gabinet> gabinety(List<ProszonyPacjentModel> pacjenci)
        {
            List<Gabinet> gabinety = new List<Gabinet>();
            foreach (var item in pacjenci)
            {
                if (!gabinety.Exists(g => g.Id == item.GABINET_ID))
                {
                    gabinety.Add(new Gabinet { Id = item.GABINET_ID, Numer = item.GABINET_NUMER, Nazwa = item.GABINET_NAZWA });
                }
            }
            return gabinety;
        }

        public class Gabinet
        {
            public int Id { get; set; }
            public string Numer { get; set; }
            public string Nazwa { get; set; }
        }
    }
}