using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Poczekalniav1.Models;

namespace Poczekalniav1.Infrastructure
{
    public class ChangedDbEventArgs : EventArgs
    {
        public ProszonyPacjentModel ZmienionyRecord { get; set; }
    }
}