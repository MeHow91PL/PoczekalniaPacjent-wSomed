using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poczekalniav1.Models
{
    public class PacjenciNaDzisModel
    {
            public int POCZEKALNIA_ID { get; set; }
            public string POCZEKALNIA_SKROT { get; set; }
            public string POCZEKALNIA_NAZWA { get; set; }
            public int NUMER_DZIENNY { get; set; }
            public string PRACOWNIK_NAZWA { get; set; }
            public int GABINET_ID { get; set; }
            public string GABINET_NAZWA { get; set; }
            public string GABINET_NUMER { get; set; }
            public DateTime GODZINA { get; set; }
    }
}