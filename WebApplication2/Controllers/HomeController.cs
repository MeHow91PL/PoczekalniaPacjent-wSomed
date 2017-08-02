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
        //DbManager db = new OracleDbManager();
        DbManager db = new LocalTestDbManager();
            
        public ActionResult Index()
        {
            List<ProszonyPacjentModel> model = db.PobierzProszonychPacjentow();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
    //public delegate void test();
    //public class ListaPacjentowManager
    //{
    //    OracDbContext db = new OracDbContext();

    //    public event test TestEvent;
    //    static List<ProszonyPacjentModel> proszeniPacjenci;

    //    public ListaPacjentowManager()
    //    {
    //        proszeniPacjenci = new List<ProszonyPacjentModel>();
    //    }

    //    public void UruchomNasluch()
    //    {
    //        while (true)
    //        {
    //            db.Database.SqlQuery
    //        }
    //    }
        
    //}
}