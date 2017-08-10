using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System.Data.Entity.Infrastructure;
using Poczekalniav1.Infrastructure;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNet.SignalR;
using Poczekalniav1.Hubs;
using System.Timers;

namespace Poczekalniav1.Controllers
{
    public class HomeController : Controller
    {
        //OracleDbManager db = new OracleDbManager();
        LocalTestDbManager db = new LocalTestDbManager();

        public ActionResult Index()
        {
            try
            {
                //bool wczytanoConfig = WczytajConfig();
                //if (!wczytanoConfig)
                //{
                //    OknoOpcji();
                //}

                OpcjeModel opcjeModel = OpcjeAplikacjiManager.GetOpcjeModel();
                WyswietlaczManager wysManager = new WyswietlaczManager();
                this.AddedNewNumber += wysManager.AddedNewNumber;
                this.RemovedNumber += wysManager.RemovedNumber;
                Timer timer1 = new Timer(1000);
                timer1.Elapsed += Timer1_Elapsed;
                timer1.Start();


                return View(opcjeModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        static List<ProszonyPacjentModel> listaWezwanych = new List<ProszonyPacjentModel>();
        static bool czyPobrany = false;
        static Queue<ProszonyPacjentModel> kolejkaWezwań = new Queue<ProszonyPacjentModel>();
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();

        public delegate void ChangedDbEventHandler(object o, ChangedDbEventArgs e);
        public event ChangedDbEventHandler AddedNewNumber;
        public event ChangedDbEventHandler RemovedNumber;

        protected virtual void OnAddedNewNumber(ProszonyPacjentModel ZmienionyRecord)
        {
            if (AddedNewNumber != null)
            {
                AddedNewNumber(this, new ChangedDbEventArgs { ZmienionyRecord = ZmienionyRecord });
            }
        }

        protected virtual void OnRemovedNumber(ProszonyPacjentModel ZmienionyRecord)
        {
            if (AddedNewNumber != null)
            {
                RemovedNumber(this, new ChangedDbEventArgs { ZmienionyRecord = ZmienionyRecord });
            }
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (db.IloscProszonychPacjentow > 0)
            {
                foreach (var item in db.PobierzProszonychPacjentow())
                {
                    if (!listaWezwanych.Any(l => l.NUMER_DZIENNY == item.NUMER_DZIENNY))
                    {
                        listaWezwanych.Add(item);
                        OnAddedNewNumber(item);
                    }
                }

                foreach (var item in listaWezwanych)
                {
                    if (!db.PobierzProszonychPacjentow().Any(l => l.NUMER_DZIENNY == item.NUMER_DZIENNY))
                    {
                        OnRemovedNumber(item);
                    }
                }
            }
            listaWezwanych = db.PobierzProszonychPacjentow().FindAll(w => listaWezwanych.Exists(l => l.NUMER_DZIENNY == w.NUMER_DZIENNY));
        }



        public void WezwijPacjenta(int numer, string gabinet)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
            db.WezwijPacjenta(numer, gabinet);
            context.Clients.All.wyswietlNumerek(db.PobierzProszonegoPacjentaPoId(numer));
            context.Clients.All.aktuListeNr(db.PobierzProszonychPacjentow());
        }
        public void ObsluzPacjenta(int numer)
        {
            db.ObsluzPacjenta(numer);
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
            context.Clients.All.aktuListeNr(db.PobierzProszonychPacjentow());
        }



        private void test(object state)
        {
            throw new NotImplementedException();
        }

        private bool WczytajConfig()
        {
            try
            {
                db.PodlaczDoBazy();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public struct DbInfo { public string DataSourcce { get; set; } public string ConnString { get; set; } }
        //public JsonResult GetDbInfo()
        //{
        //    DbInfo inf = db.GetDbInfo();
        //    return Json(inf, JsonRequestBehavior.AllowGet);
        //}

        public PartialViewResult OknoOpcji()
        {
            OpcjeModel model = OpcjeAplikacjiManager.GetOpcjeModel();
            return PartialView(model);
        }

        public void testt(object state)
        {

        }

        [HttpPost]
        public ActionResult ZapiszOpcje(OpcjeModel model)
        {
            try
            {
                OpcjeAplikacjiManager.DatabaseConnectionString = model.DatabaseConnString.ConnectionString;
                OpcjeAplikacjiManager.BackgroundColor = model.BackgroundColor;
                OpcjeAplikacjiManager.BackgroundImageOpacity = model.BackgroundImgOpacity;
                HttpPostedFileBase img = Request.Files[0];
                if (img != null && img.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images"),
                                               Path.GetFileName(img.FileName));
                    img.SaveAs(path);
                    OpcjeAplikacjiManager.BackgroundImage = img.FileName;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }







        public bool CzyPobrany()
        {
            return czyPobrany;
        }
    }
}