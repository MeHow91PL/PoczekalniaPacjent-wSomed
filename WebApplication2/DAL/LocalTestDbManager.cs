using System;
using System.Collections.Generic;
using Poczekalniav1.DAL;
using Poczekalniav1.Models;

namespace Poczekalniav1.DAL
{
    internal class LocalTestDbManager : DbManager
    {
        static List<ProszonyPacjentModel> testowaLista = new List<ProszonyPacjentModel>
        {
            //new ProszonyPacjentModel{ GABINET_NAZWA = "Gabinet 20", NUMER_DZIENNY = 1},
            //new ProszonyPacjentModel{ GABINET_NAZWA = "Gabinet 21", NUMER_DZIENNY = 2},
            new ProszonyPacjentModel{ GABINET_NAZWA = "Gabinet 20", NUMER_DZIENNY = 3}
        };

        public override int IloscProszonychPacjentow { get;}

        public override Queue<ProszonyPacjentModel> KolejkaWezwan => throw new NotImplementedException();

        public override ProszonyPacjentModel PobierzProszonegoPacjentaPoId(int numerDzienny)
        {
            if (!testowaLista.Exists(n => n.NUMER_DZIENNY == numerDzienny))
            {
                throw new NullReferenceException("Taki numer nie jest w tym momencie wzywany do gabinetu");
            }
            return testowaLista.Find(p => p.NUMER_DZIENNY == numerDzienny);
        }

        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow()
        {
            return testowaLista;
        }

        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow(int idPoczekalni)
        {
            throw new NotImplementedException();
        }

        public override List<ProszonyPacjentModel> PobierzProszonychPacjentow(string nazwaGabinetu)
        {
            return testowaLista.FindAll(p => p.GABINET_NAZWA == nazwaGabinetu);
        }
    }
}