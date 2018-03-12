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
    public class TribunalController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Tribunais.OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.Tribunais.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult ConsultarTribunal(int? pagina, string DescricaoTribunal = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var Tribunal = new object();
            if (!string.IsNullOrEmpty(DescricaoTribunal))
            {
                Tribunal = db.Tribunais
                    .Where(e => e.Descricao.ToUpper().Contains(DescricaoTribunal.ToUpper()))
                    .OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            else
            {
                Tribunal = db.Tribunais.OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", Tribunal);
        }

        // GET: Tribunal
        [Authorize(Roles = "Administrador,Usuario")]
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var tribunais = db.Tribunais.AsQueryable();

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DescricaoTribunalParam = string.IsNullOrEmpty(ordenacao) ? "DescricaoTribunal_Desc" : "";
            ViewBag.EnderecoTribunalParam = ordenacao == "EnderecoTribunal" ? "EnderecoTribunal_Desc" : "EnderecoTribunal";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            switch (ordenacao)
            {
                case "DescricaoTribunal_Desc":
                    tribunais = db.Tribunais.OrderByDescending(s => s.Descricao);
                    break;
                case "EnderecoTribunal":
                    tribunais = db.Tribunais.OrderBy(s => s.Endereco);
                    break;
                case "EnderecoTribunal_Desc":
                    tribunais = db.Tribunais.OrderByDescending(s => s.Endereco);
                    break;
                default:
                    tribunais = db.Tribunais.OrderBy(s => s.Descricao);
                    break;
            }

            return View(tribunais.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Tribunal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tribunal tribunal = db.Tribunais.Find(id);
            if (tribunal == null)
            {
                return HttpNotFound();
            }
            return View(tribunal);
        }

        // GET: Tribunal/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tribunal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TribunalID,Descricao,Endereco")] Tribunal tribunal)
        {
            if (ModelState.IsValid)
            {
                db.Tribunais.Add(tribunal);
                db.SaveChanges();
                TempData["Mensagem"] = "Tribunal Cadastrado Com Sucesso!";
                return RedirectToAction("Index");
            }

            return View(tribunal);
        }

        // GET: Tribunal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tribunal tribunal = db.Tribunais.Find(id);
            if (tribunal == null)
            {
                return HttpNotFound();
            }
            return View(tribunal);
        }

        // POST: Tribunal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TribunalID,Descricao,Endereco")] Tribunal tribunal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tribunal).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Tribunal Atualizado Com Sucesso!";
                return RedirectToAction("Index");
            }
            return View(tribunal);
        }

        // GET: Tribunal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tribunal tribunal = db.Tribunais.Find(id);
            if (tribunal == null)
            {
                return HttpNotFound();
            }
            return View(tribunal);
        }

        // POST: Tribunal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tribunal tribunal = db.Tribunais.Find(id);
            db.Tribunais.Remove(tribunal);
            db.SaveChanges();
            TempData["Mensagem"] = "Tribunal Excluido Com Sucesso!";
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
