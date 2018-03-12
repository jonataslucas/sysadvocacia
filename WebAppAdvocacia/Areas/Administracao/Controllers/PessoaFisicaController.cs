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
using WebAppAdvocacia.Models.ViewModels;
using System.IO;
using Rotativa;
namespace Areas.Administracao.Controllers
{
    public class PessoaFisicaController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.PessoasFisicas.OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.PessoasFisicas.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult VerificaEmail(string Email)
        {
            return Json(db.PessoasFisicas
                .All(p => p.Email.ToLower() != Email.ToLower())
                , JsonRequestBehavior.AllowGet);

        }

        public ActionResult VerificaEmailAtualiza(int PessoaID, string Email)
        {
            var pessoaFisica = db.PessoasFisicas.Find(PessoaID);
            bool resultado = true;
            if (pessoaFisica.Email.ToLower() != Email.ToLower())
            {
                resultado = db.PessoasFisicas
                .All(p => p.Email.ToLower() != Email.ToLower());
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ConsultarPessoaFisica(int? pagina, string Nome = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var PessoaFisica = new object();
            if (!string.IsNullOrEmpty(Nome))
            {
                PessoaFisica = db.PessoasFisicas
                    .Where(e => e.Nome.ToUpper().Contains(Nome.ToUpper()))
                    .OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                PessoaFisica = db.PessoasFisicas.OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", PessoaFisica);
        }

        // GET: PessoaFisica
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var PessoasFisicas = db.PessoasFisicas.AsQueryable();

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_Desc" : "";
            ViewBag.EnderecoParam = ordenacao == "Endereco" ? "Endereco_Desc" : "Endereco";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            switch (ordenacao)
            {
                case "Nome_Desc":
                    PessoasFisicas = db.PessoasFisicas.OrderByDescending(s => s.Nome);
                    break;
                case "Endereco":
                    PessoasFisicas = db.PessoasFisicas.OrderBy(s => s.Endereco);
                    break;
                case "Endereco_Desc":
                    PessoasFisicas = db.PessoasFisicas.OrderByDescending(s => s.Endereco);
                    break;
                default:
                    PessoasFisicas = db.PessoasFisicas.OrderBy(s => s.Nome);
                    break;
            }

            return View(PessoasFisicas.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: PessoaFisica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaFisica = db.Pessoas.Find(id);
            if (pessoaFisica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaFisica);
        }

        // GET: PessoaFisica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaFisica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CadastroPessoaFisicaViewModel pessoaFisicaViewModel)
        {
            if (ModelState.IsValid)
            {

                var pessoaFisica = new PessoaFisica
                {
                    PessoaID = pessoaFisicaViewModel.PessoaID,
                    Bairro = pessoaFisicaViewModel.Bairro,
                    CEP = pessoaFisicaViewModel.CEP,
                    Cidade = pessoaFisicaViewModel.Cidade,
                    CPF = pessoaFisicaViewModel.CPF,
                    Email = pessoaFisicaViewModel.Email,
                    Endereco = pessoaFisicaViewModel.Endereco,
                    Nome = pessoaFisicaViewModel.Nome,
                    RG = pessoaFisicaViewModel.RG,
                    Telefone = pessoaFisicaViewModel.Telefone,
                    UF = pessoaFisicaViewModel.UF,
                    Foto = pessoaFisicaViewModel.Foto
                };

                db.Pessoas.Add(pessoaFisica);
                db.SaveChanges();

                if (pessoaFisica.Foto.ContentLength > 0)
                {
                    var nomeArquivo = pessoaFisica.PessoaID.ToString() + ".jpg";
                    var caminho = Path.Combine(Server.MapPath("~/Content/Images"), nomeArquivo);
                    pessoaFisica.Foto.SaveAs(caminho);
                }

                TempData["Mensagem"] = "Pessoa Fisica Cadastrada Com Sucesso!";
                return RedirectToAction("Index");
            }

            return View(pessoaFisicaViewModel);
        }

        // GET: PessoaFisica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaFisica = db.PessoasFisicas.Find(id);

            AtualizaPessoaFisicaViewModel pessoaFisicaVM =
            new AtualizaPessoaFisicaViewModel()
            {
                PessoaID = pessoaFisica.PessoaID,
                Bairro = pessoaFisica.Bairro,
                CEP = pessoaFisica.CEP,
                Cidade = pessoaFisica.Cidade,
                CPF = pessoaFisica.CPF,
                Email = pessoaFisica.Email,
                Endereco = pessoaFisica.Endereco,
                Nome = pessoaFisica.Nome,
                RG = pessoaFisica.RG,
                Telefone = pessoaFisica.Telefone,
                UF = pessoaFisica.UF

            };

            if (pessoaFisica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaFisicaVM);
        }

        // POST: PessoaFisica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AtualizaPessoaFisicaViewModel pessoaFisicaViewModel)
        {
            if (ModelState.IsValid)
            {

                var pessoaFisica = new PessoaFisica
                {
                    PessoaID = pessoaFisicaViewModel.PessoaID,
                    Bairro = pessoaFisicaViewModel.Bairro,
                    CEP = pessoaFisicaViewModel.CEP,
                    Cidade = pessoaFisicaViewModel.Cidade,
                    CPF = pessoaFisicaViewModel.CPF,
                    Email = pessoaFisicaViewModel.Email,
                    Endereco = pessoaFisicaViewModel.Endereco,
                    Nome = pessoaFisicaViewModel.Nome,
                    RG = pessoaFisicaViewModel.RG,
                    Telefone = pessoaFisicaViewModel.Telefone,
                    UF = pessoaFisicaViewModel.UF

                };

                db.Entry(pessoaFisica).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Pessoa Fisica Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            return View(pessoaFisicaViewModel);
        }

        // GET: PessoaFisica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaFisica = db.Pessoas.Find(id);
            if (pessoaFisica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaFisica);
        }

        // POST: PessoaFisica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pessoaFisica = db.Pessoas.Find(id);
            var nomeArquivo = pessoaFisica.PessoaID.ToString() + ".jpg";
            var caminho = Path.Combine(Server.MapPath("~/Content/Images"), nomeArquivo);
            if (System.IO.File.Exists(caminho))
            {
                System.IO.File.Delete(caminho);
            }
            db.Pessoas.Remove(pessoaFisica);
            db.SaveChanges();
            TempData["Mensagem"] = "Pessoa Fisica Excluida Com Sucesso!";
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
