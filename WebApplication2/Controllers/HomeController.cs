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
        OracleDbManager db = new OracleDbManager();
        //LocalTestDbManager db = new LocalTestDbManager();
        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
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
                return View(opcjeModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        //public void WezwijPacjenta(int numer, string gabinet)
        //{
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
        //    db.WezwijPacjenta(numer, gabinet);
        //    context.Clients.All.wyswietlNumerek(db.PobierzProszonegoPacjentaPoId(numer));
        //    context.Clients.All.aktuListeNr(db.PobierzProszonychPacjentow());
        //}
        //public void ObsluzPacjenta(int numer)
        //{
        //    db.ObsluzPacjenta(numer);
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
        //    context.Clients.All.aktuListeNr(db.PobierzProszonychPacjentow());
        //}



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
                HttpPostedFileBase img = Request.Files[0];
                if (img != null && img.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Images"),
                                               Path.GetFileName(img.FileName));
                    img.SaveAs(path);
                    OpcjeAplikacjiManager.BackgroundImage = img.FileName;
                }
                OpcjeAplikacjiManager.BackgroundImageOpacity = model.BackgroundImgOpacity;
                OpcjeAplikacjiManager.BackgroundBlur = model.BackgroundBlur;
                OpcjeAplikacjiManager.OnlyWithNumberQueue = model.OnlyWithNumberQueue;
                OpcjeAplikacjiManager.WezwaniPacjenci = model.WezwaniPacjenci;
                OpcjeAplikacjiManager.KolejkaWezwanych = model.KolejkaWezwanych;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}