using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System.Data.Entity.Infrastructure;

namespace Poczekalniav1.Controllers
{
    public class HomeController : Controller
    {
        DbManager db = new OracleDbManager();
        //DbManager db = new LocalTestDbManager();

        public ActionResult Index()
        {
            List<ProszonyPacjentModel> model = db.PobierzProszonychPacjentow();
            return View(model);
        }

        static List<ProszonyPacjentModel> listaWezwanych = new List<ProszonyPacjentModel>();
        static bool czyPobrany = false;
        static Queue<ProszonyPacjentModel> kolejkaWezwań = new Queue<ProszonyPacjentModel>();

        public PartialViewResult WyswietlNumerek()
        {
            czyPobrany = false;
            ProszonyPacjentModel model = null;
            if (kolejkaWezwań.Count > 0)
            {
                ProszonyPacjentModel peek = kolejkaWezwań.Peek();
                if (listaWezwanych.Exists(p => p.NUMER_DZIENNY == peek.NUMER_DZIENNY))
                {
                    kolejkaWezwań.Dequeue();
                }
                else if (kolejkaWezwań.Count > 0)
                {
                    model = kolejkaWezwań.Dequeue();
                    listaWezwanych.Add(model);
                    czyPobrany = true;
                }
            }
            else
            {
                listaWezwanych = db.PobierzProszonychPacjentow().FindAll(w => listaWezwanych.Exists(l => l.NUMER_DZIENNY == w.NUMER_DZIENNY));
                kolejkaWezwań = db.KolejkaWezwan;
            }

            return PartialView(model);
        }

        public bool CzyPobrany()
        {
            return czyPobrany;
        }
    }
}