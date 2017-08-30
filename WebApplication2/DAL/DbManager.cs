using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poczekalniav1.Models;

namespace Poczekalniav1.DAL
{
    public abstract class DbManager
    {
        public abstract int IloscProszonychPacjentow { get;  }
        public abstract Queue<ProszonyPacjentModel> KolejkaWezwan { get; }
        public abstract bool CzyPolaczonoPoprawnie();
        public abstract ProszonyPacjentModel PobierzProszonegoPacjentaPoId(int numerDzienny);
        public abstract List<ProszonyPacjentModel> PobierzProszonychPacjentow();
        public abstract List<ProszonyPacjentModel> PobierzProszonychPacjentow(int idPoczekalni);
        public abstract List<ProszonyPacjentModel> PobierzProszonychPacjentow(string nazwaGabinetu);
    }

    
}
