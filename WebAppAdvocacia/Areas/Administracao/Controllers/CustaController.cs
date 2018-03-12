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
using System.Globalization;
using System.Threading;
namespace Areas.Administracao.Controllers
{
    public class CustaController : Controller
    {//TODO: Alterar Metodo

        /// <summary>
        /// Nova alteraççao
        /// </summary>
        private ContextoEF db = new ContextoEF();

        protected void Application_BeginRequest()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            // CultureInfo culture = new CultureInfo("en-Us");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Custas.OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.Custas.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult VisualizarCustas(int? pagina, string NumeroProcesso = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var Custa = new object();
            if (!string.IsNullOrEmpty(NumeroProcesso))
            {
                Custa = db.Custas
                    .Where(e => e.ProcessoID.ToString() == (NumeroProcesso))
                    .OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                Custa = db.Custas.OrderBy(e => e.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", Custa);
        }



        // GET: Custa
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var custas = db.Custas.Include(c => c.Processo);

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DescricaoParam = string.IsNullOrEmpty(ordenacao) ? "Descricao" : "";
            ViewBag.DataParam = ordenacao == "Data" ? "Data_Desc" : "Data";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
          //var Custa = from e in db.Custas select e;
            switch (ordenacao)
            {
                case "Descricao":
                    custas = db.Custas.Include(e => e.Processo).OrderByDescending(s => s.Descricao);
                    break;
                case "Data":
                    custas = db.Custas.Include(e => e.Processo).OrderBy(s => s.Data);
                    break;
                case "Data_Desc":
                    custas = db.Custas.Include(e => e.Processo).OrderByDescending(s => s.Data);
                    break;
                default:
                    custas = db.Custas.Include(e => e.Processo).OrderBy(s => s.Descricao);
                    break;
            }
            return View(custas.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Custa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custa custa = db.Custas.Find(id);
            if (custa == null)
            {
                return HttpNotFound();
            }
            return View(custa);
        }

        // GET: Custa/Create
        public ActionResult Create()
        {
           
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao");
            return View();
        }

        // POST: Custa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustaID,Data,Descricao,Valor,ProcessoID")] Custa custa)
        {
            
            if (ModelState.IsValid)
            {
                db.Custas.Add(custa);
                db.SaveChanges();
                TempData["Mensagem"] = "Custa Cadastrada Com Sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", custa.ProcessoID);
            return View(custa);
        }

        // GET: Custa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custa custa = db.Custas.Find(id);
            if (custa == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", custa.ProcessoID);
            return View(custa);
        }

        // POST: Custa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustaID,Data,Descricao,Valor,ProcessoID")] Custa custa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custa).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Custa Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.ProcessoID = new SelectList(db.Processos, "ProcessoID", "Descricao", custa.ProcessoID);
            return View(custa);
        }

        // GET: Custa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custa custa = db.Custas.Find(id);
            if (custa == null)
            {
                return HttpNotFound();
            }
            return View(custa);
        }

        // POST: Custa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Custa custa = db.Custas.Find(id);
            db.Custas.Remove(custa);
            db.SaveChanges();
            TempData["Mensagem"] = "Custa Excluida Com Sucesso!";
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
