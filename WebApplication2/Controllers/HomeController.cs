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
        DbManager db = new LocalTestDbManager();
        Timer timer1 = new Timer(10000);

        public ActionResult Index()
        {
            try
            {
                bool wczytanoConfig = WczytajConfig();
                if (!wczytanoConfig)
                {
                    OknoOpcji();
                }
                OpcjeModel opcjeModel = OpcjeAplikacjiManager.GetOpcjeModel();
                timer1.Elapsed += Timer1_Elapsed;
                timer1.Start();

                return View(opcjeModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            WyswietlNumerek();
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



        static List<ProszonyPacjentModel> listaWezwanych = new List<ProszonyPacjentModel>();
        static bool czyPobrany = false;
        static Queue<ProszonyPacjentModel> kolejkaWezwań = new Queue<ProszonyPacjentModel>();

        public void WyswietlNumerek()
        {
            //czyPobrany = false;
            //ProszonyPacjentModel model = null;
            //if (kolejkaWezwań.Count > 0)
            //{
            //    ProszonyPacjentModel peek = kolejkaWezwań.Peek();
            //    if (listaWezwanych.Exists(p => p.NUMER_DZIENNY == peek.NUMER_DZIENNY))
            //    {
            //        kolejkaWezwań.Dequeue();
            //    }
            //    else if (kolejkaWezwań.Count > 0)
            //    {
            //        model = kolejkaWezwań.Dequeue();
            //        listaWezwanych.Add(model);
            //        czyPobrany = true;
            //        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
            //        context.Clients.All.wyswietlNumerek(PartialView(model));
            //    }
            //}
            //else
            //{
            //    listaWezwanych = db.PobierzProszonychPacjentow().FindAll(w => listaWezwanych.Exists(l => l.NUMER_DZIENNY == w.NUMER_DZIENNY));
            //    kolejkaWezwań = db.KolejkaWezwan;
            //}

            if (db.PobierzProszonychPacjentow()[0] != null)
            {

                ProszonyPacjentModel model = db.PobierzProszonychPacjentow()[0];
                
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
                context.Clients.All.wyswietlNumerek(model);
            }


        }

        public bool CzyPobrany()
        {
            return czyPobrany;
        }
    }
}