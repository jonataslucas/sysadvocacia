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
    public class AudienciaController : Controller
    {
        //TODO: Alterar o nome do método.
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Audiencias.OrderBy(e => e.ProcessoID).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.Audiencias.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult ConsultarAudiencia(int? pagina, string ProcessoID = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var Audiencia = new object();
            if (!string.IsNullOrEmpty(ProcessoID))
            {
                Audiencia = db.Audiencias
                    .Where(e => e.ProcessoID.ToString() == (ProcessoID))
                    .OrderBy(e => e.ProcessoID).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                Audiencia = db.Audiencias.OrderBy(e => e.ProcessoID).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", Audiencia);
        }

        // GET: Audiencia
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var audiencias = db.Audiencias.Include(a => a.Processo);

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.ProcessoID = string.IsNullOrEmpty(ordenacao) ? "ProcessoID_Desc" : "";
            ViewBag.Data = ordenacao == "Data" ? "Data_Desc" : "Data";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            //var Processo = from e in db.Processos select e;
            switch (ordenacao)
            {
                case "ProcessoID_Desc":
                    audiencias = db.Audiencias.Include(a => a.Processo).OrderByDescending(a => a.ProcessoID);
                    break;
                case "Data":
                    audiencias = db.Audiencias.Include(e => e.Processo).OrderBy(a => a.Data);
                    break;
                case "Data_Desc":
                    audiencias = db.Audiencias.Include(e => e.Processo).OrderByDescending(a => a.Data);
                    break;
                default:
                    audiencias = db.Audiencias.Include(e => e.Processo).OrderBy(a => a.ProcessoID);
                    break;
            }
            return View(audiencias.ToPagedList(numeroPagina, tamanhoPagina));

        }

        // GET: Audiencia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audiencia audiencia = db.Audiencias.Find(id);
            if (audiencia == null)
            {
                return HttpNotFound();
            }
            return View(audiencia);
        }

        // GET: Audiencia/Create
        public ActionResult Create()
        {
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao");
            return View();
        }

        // POST: Audiencia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AudienciaID,Data,Parecer,ProcessoID")] Audiencia audiencia)
        {
            if (ModelState.IsValid)
            {
                db.Audiencias.Add(audiencia);
                db.SaveChanges();
                TempData["Mensagem"] = "Audiencia Cadastrada Com Sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", audiencia.ProcessoID);
            return View(audiencia);
        }

        // GET: Audiencia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audiencia audiencia = db.Audiencias.Find(id);
            if (audiencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", audiencia.ProcessoID);
            return View(audiencia);
        }

        // POST: Audiencia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AudienciaID,Data,Parecer,ProcessoID")] Audiencia audiencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audiencia).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Audiencia Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", audiencia.ProcessoID);
            return View(audiencia);
        }

        // GET: Audiencia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Audiencia audiencia = db.Audiencias.Find(id);
            if (audiencia == null)
            {
                return HttpNotFound();
            }
            return View(audiencia);
        }

        // POST: Audiencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Audiencia audiencia = db.Audiencias.Find(id);
            db.Audiencias.Remove(audiencia);
            db.SaveChanges();
            TempData["Mensagem"] = "Audiencia Excluida Com Sucesso!";
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
