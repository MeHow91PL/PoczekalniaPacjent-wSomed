using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poczekalniav1.Models;

namespace Poczekalniav1.Hubs
{
    public class TestHub : Hub
    {
        public void Send(ProszonyPacjentModel pac)
        {
            Clients.All.wyswietlNumerek(pac);
            Clients.All.ukryjNr(pac);
        }

        public void AktuListeNr(TimeSpan list)
        {
            Clients.All.aktuListeNr(list);
        }


    }
}