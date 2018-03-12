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
    public class ProcessoController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Processos.OrderBy(e => e.Situacao).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.Processos.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult ConsultarProcesso(int? pagina, string NumeroProcesso = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var Processo = new object();
            if (!string.IsNullOrEmpty(NumeroProcesso))
            {
                Processo = db.Processos
                    .Where(e => e.NumeroProcesso.ToString() == (NumeroProcesso))
                    .OrderBy(e => e.NumeroProcesso).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                Processo = db.Processos.OrderBy(e => e.NumeroProcesso).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", Processo);
        }

        // GET: Processo
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var processos = db.Processos.Include(p => p.Pessoa).Include(p => p.Vara);


            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.SituacaoParam = string.IsNullOrEmpty(ordenacao) ? "Situacao_Desc" : "";
            ViewBag.DataAberturaParam = ordenacao == "DataAbertura" ? "DataAbertura_Desc" : "DataAbertura";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
           //var Processo = from e in db.Processos select e;
            switch (ordenacao)
            {
                case "Situacao_Desc":
                    processos = db.Processos.Include(e => e.Pessoa).Include(e => e.Vara).OrderByDescending(s => s.Situacao);
                    break;
                case "DataAbertura":
                    processos = db.Processos.Include(e => e.Pessoa).Include(e => e.Vara).OrderBy(s => s.DataAbertura);
                    break;
                case "DataAbertura_Desc":
                    processos = db.Processos.Include(e => e.Pessoa).Include(e => e.Vara).OrderByDescending(s => s.DataAbertura);
                    break;
                default:
                    processos = db.Processos.Include(e => e.Pessoa).Include(e => e.Vara).OrderBy(s => s.Situacao);
                    break;
            }
            return View(processos.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Processo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Processo processo = db.Processos.Find(id);
            if (processo == null)
            {
                return HttpNotFound();
            }
            return View(processo);
        }

        // GET: Processo/Create
        public ActionResult Create()
        {
            ViewBag.PessoaID = new SelectList(db.Pessoas, "PessoaID", "Nome");
            ViewBag.VaraID = new SelectList(db.Varas, "VaraID", "Descricao");
            return View();
        }

        // POST: Processo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Processo processo)
        {
            if(processo.DataAbertura > processo.DataConclusao)
            {
                ModelState.AddModelError("", "A data de abertura deve ser menor ou igual a data de conclusão");
            }

            if (ModelState.IsValid)
            {
                db.Processos.Add(processo);
                db.SaveChanges();
                TempData["Mensagem"] = "Processo Cadastrado Com Sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.PessoaID = new SelectList(db.Pessoas, "PessoaID", "Nome", processo.PessoaID);
            ViewBag.VaraID = new SelectList(db.Varas, "VaraID", "Descricao", processo.VaraID);
            return View(processo);
        }

        // GET: Processo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Processo processo = db.Processos.Find(id);
            if (processo == null)
            {
                return HttpNotFound();
            }
            ViewBag.PessoaID = new SelectList(db.Pessoas, "PessoaID", "Nome", processo.PessoaID);
            ViewBag.VaraID = new SelectList(db.Varas, "VaraID", "Descricao", processo.VaraID);
            return View(processo);
        }

        // POST: Processo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProcessoID,NumeroProcesso,DataAbertura,DataConclusao,Situacao,PessoaID,VaraID,Descricao")] Processo processo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(processo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Processo Atualizado Com Sucesso!";
                return RedirectToAction("Index");
            }
            ViewBag.PessoaID = new SelectList(db.Pessoas, "PessoaID", "Nome", processo.PessoaID);
            ViewBag.VaraID = new SelectList(db.Varas, "VaraID", "Descricao", processo.VaraID);
            return View(processo);
        }

        // GET: Processo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Processo processo = db.Processos.Find(id);
            if (processo == null)
            {
                return HttpNotFound();
            }
            return View(processo);
        }

        // POST: Processo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Processo processo = db.Processos.Find(id);
            db.Processos.Remove(processo);
            db.SaveChanges();
            TempData["Mensagem"] = "Processo Excluido Com Sucesso!";
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
