using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poczekalniav1.DAL;
using Poczekalniav1.Models;
using System.Data.Entity.Infrastructure;
using System.Threading;
using Poczekalniav1.Infrastructure;
using System.Diagnostics;
using System.IO;

namespace Poczekalniav1.Controllers
{
    public class HomeController : Controller
    {
        OracleDbManager db = new OracleDbManager();
        //DbManager db = new LocalTestDbManager();

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
                return View(opcjeModel);
            }
            catch (Exception ex)
            {
                throw;
            }
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

        public struct DbInfo { public string DataSourcce { get; set; } public string ConnString { get; set; } }
        public JsonResult GetDbInfo()
        {
            DbInfo inf = db.GetDbInfo();
            return Json(inf, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult OknoOpcji()
        {
            OpcjeModel model = OpcjeAplikacjiManager.GetOpcjeModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ZapiszOpcje()
        {
            int i = Request.Files.Count;
            try
            {
                //OpcjeAplikacjiManager.BackgroundColor = model.BackgroundColor;
                //OpcjeAplikacjiManager.DatabaseConnectionString = model.DatabaseConnString.ConnectionString;
                //if (model.BackgroundImg != null && model.BackgroundImg.ContentLength > 0)
                //{
                //    string path = Path.Combine(Server.MapPath("~/Images"),
                //                               Path.GetFileName(model.BackgroundImg.FileName));
                //    model.BackgroundImg.SaveAs(path);
                //    OpcjeAplikacjiManager.BackgroundImage = model.BackgroundImg.FileName;
                //}

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