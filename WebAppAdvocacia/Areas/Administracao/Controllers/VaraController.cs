using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppAdvocacia.Models;
using PagedList;
using Rotativa;
namespace Areas.Administracao.Controllers
{
    public class VaraController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Varas.OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.Varas.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult ConsultarVara(int? pagina, string DescricaoVara = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var Vara = new object();
            if(!string.IsNullOrEmpty(DescricaoVara))
            {
                Vara = db.Varas
                    .Where(e => e.Descricao.ToUpper().Contains(DescricaoVara.ToUpper()))
                    .OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            else
            {
                Vara = db.Varas.OrderBy(s => s.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", Vara);
        }

        // GET: Vara
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var varas = db.Varas.Include(v => v.Tribunal);

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DescricaoVaraParam = string.IsNullOrEmpty(ordenacao) ? "DescricaoVara_Desc" : "";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            switch (ordenacao)
            {
                case "DescricaoVara_Desc":
                    varas = db.Varas.Include(v => v.Tribunal).OrderByDescending(s => s.Descricao);
                    break;
                default:
                    varas = db.Varas.Include(v => v.Tribunal).OrderBy(s => s.Descricao);
                    break;
            }


            return View(varas.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Vara/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vara vara = db.Varas.Find(id);
            if (vara == null)
            {
                return HttpNotFound();
            }
            return View(vara);
        }

        // GET: Vara/Create
        public ActionResult Create()
        {
            ViewBag.TribunalID = new SelectList(db.Tribunais, "TribunalID", "Descricao");
            return View();
        }

        // POST: Vara/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VaraID,Descricao,TribunalID")] Vara vara)
        {
            if (ModelState.IsValid)
            {
                db.Varas.Add(vara);
                db.SaveChanges();
                TempData["Mensagem"] = "Vara Cadastrada Com Sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.TribunalID = new SelectList(db.Tribunais, "TribunalID", "Descricao", vara.TribunalID);
            return View(vara);
        }

        // GET: Vara/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vara vara = db.Varas.Find(id);
            if (vara == null)
            {
                return HttpNotFound();
            }
            ViewBag.TribunalID = new SelectList(db.Tribunais, "TribunalID", "Descricao", vara.TribunalID);
            return View(vara);
        }

        // POST: Vara/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VaraID,Descricao,TribunalID")] Vara vara)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vara).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Vara Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.TribunalID = new SelectList(db.Tribunais, "TribunalID", "Descricao", vara.TribunalID);
            return View(vara);
        }

        // GET: Vara/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vara vara = db.Varas.Find(id);
            if (vara == null)
            {
                return HttpNotFound();
            }
            return View(vara);
        }

        // POST: Vara/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vara vara = db.Varas.Find(id);
            db.Varas.Remove(vara);
            db.SaveChanges();
            TempData["Mensagem"] = "Vara Excluida Com Sucesso!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
