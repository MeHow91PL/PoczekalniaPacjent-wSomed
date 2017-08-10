using System;
using System.Linq;
using System.Collections.Generic;
using Poczekalniav1.DAL;
using Poczekalniav1.Models;

namespace Poczekalniav1.DAL
{
    internal class LocalTestDbManager : DbManager
    {
        static  List<ProszonyPacjentModel> testowaLista = new List<ProszonyPacjentModel>();

        public override int IloscProszonychPacjentow
        {
            get { return testowaLista.Count; }
        }

        private Queue<ProszonyPacjentModel> _kolejkaWezwan ;
        public override Queue<ProszonyPacjentModel> KolejkaWezwan
        {
            get
            {
                _kolejkaWezwan = new Queue<ProszonyPacjentModel>();
                if (IloscProszonychPacjentow > 0)
                {
                    foreach (var item in PobierzProszonychPacjentow())
                    {
                        _kolejkaWezwan.Enqueue(item);
                    }
                }
                return _kolejkaWezwan;
            }
        }

        public void WezwijPacjenta(int nr, string nazw)
        {
            testowaLista.Add(new ProszonyPacjentModel { GABINET_NAZWA = nazw, NUMER_DZIENNY = nr });
        }

        public void ObsluzPacjenta(int nr)
        {
            testowaLista.Remove(testowaLista.Find(p => p.NUMER_DZIENNY == nr));
        }

        public override void PodlaczDoBazy()
        {
            throw new NotImplementedException();
        }

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
            List<ProszonyPacjentModel> t = testowaLista.FindAll(p => true);
            return t;
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