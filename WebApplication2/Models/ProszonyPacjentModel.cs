using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poczekalniav1.Models
{
    public class ProszonyPacjentModel
    {
        public int GABINET_ID { get; set; }
        public string GABINET_NAZWA { get; set; }
        public string GABINET_NUMER { get; set; }
        public int NUMER_DZIENNY { get; set; }
        public string GODZINA { get; set; }
    }
}