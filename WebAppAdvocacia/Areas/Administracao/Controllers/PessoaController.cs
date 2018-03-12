using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppAdvocacia.Models;
using WebAppAdvocacia.Models.ViewModels;
namespace Areas.Administracao.Controllers
{
    public class PessoaController : Controller
    {
        private ContextoEF db = new ContextoEF();

        // GET: Pessoa
        public ActionResult Index()
        {
            return View(db.Pessoas.ToList());
        }

        // GET: Pessoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // GET: Pessoa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pessoa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PessoaID,Nome,Endereco,Telefone,CEP,Bairro,Cidade,UF,Email")] CadastroPessoaFisicaViewModel pessoaVM)
        {
            var pessoa = new Pessoa()

            {
                PessoaID = pessoaVM.PessoaID,
                Nome = pessoaVM.Nome,
                Endereco = pessoaVM.Endereco,
                Telefone = pessoaVM.Telefone,
                CEP = pessoaVM.CEP,
                Bairro = pessoaVM.Bairro,
                Cidade = pessoaVM.Cidade,
                UF = pessoaVM.UF,
                Email = pessoaVM.Email
            };


            if (ModelState.IsValid)
            {
                db.Pessoas.Add(pessoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pessoa);
        }

        // GET: Pessoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);

            AtualizaPessoaFisicaViewModel pessoaVM = new AtualizaPessoaFisicaViewModel()
            {
                PessoaID = pessoa.PessoaID,
                Nome = pessoa.Nome,
                Endereco = pessoa.Endereco,
                Telefone = pessoa.Telefone,
                CEP = pessoa.CEP,
                Bairro = pessoa.Bairro,
                Cidade = pessoa.Cidade,
                UF = pessoa.UF,
                Email = pessoa.Email
            }; 



            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoaVM);
        }

        // POST: Pessoa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PessoaID,Nome,Endereco,Telefone,CEP,Bairro,Cidade,UF,Email")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pessoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }

        // GET: Pessoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pessoa pessoa = db.Pessoas.Find(id);
            db.Pessoas.Remove(pessoa);
            db.SaveChanges();
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
