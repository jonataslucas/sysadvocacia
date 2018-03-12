using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Areas.Administracao.Controllers
{
    public class TesteController : Controller
    {
        // GET: Administracao/Teste

        [Authorize(Roles = "Administrador,Usuario")]
        public ActionResult Index()
        {
            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (User.IsInRole("Usuario"))
            {
                return RedirectToAction("Index", "Tribunal");
            }
            return View();
        }
    }
}