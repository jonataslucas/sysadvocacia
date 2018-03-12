using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Jonatas.DiagramaDeClasse;
namespace WebAppAdvocacia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // DiagramaDeClasse.GerarDiagrama();
            return View();
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
}