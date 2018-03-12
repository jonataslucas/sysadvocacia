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
    //TODO:
    public class PessoaJuridicaController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.PessoasJuridicas.OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult GerarPDFEmDetalhes(int? id)
        {
            var modelo = db.PessoasJuridicas.Find(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressaoEmDetalhes",
                Model = modelo
            };

            return pdf;
        }

        public ActionResult VerificaEmail(string Email)
        {
            return Json(db.PessoasJuridicas
                .All(p => p.Email.ToLower() != Email.ToLower())
                , JsonRequestBehavior.AllowGet);

        }

        public ActionResult VerificaEmailAtualiza(int PessoaID, string Email)
        {
            var pessoaJuridica = db.PessoasJuridicas.Find(PessoaID);
            bool resultado = true;
            if (pessoaJuridica.Email.ToLower() != Email.ToLower())
            {
                resultado = db.PessoasJuridicas
                .All(p => p.Email.ToLower() != Email.ToLower());
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsultarPessoaJuridica(int? pagina, string Nome = null)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var PessoaJuridica = new object();
            if (!string.IsNullOrEmpty(Nome))
            {
                PessoaJuridica = db.PessoasJuridicas
                    .Where(e => e.Nome.ToUpper().Contains(Nome.ToUpper()))
                    .OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                PessoaJuridica = db.PessoasJuridicas.OrderBy(e => e.Nome).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", PessoaJuridica);
        }

        // GET: PessoaJuridica
        public ActionResult Index(string ordenacao, int? pagina)
        {
            var PessoasJuridicas = db.PessoasJuridicas.AsQueryable();

            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_Desc" : "";
            ViewBag.EnderecoParam = ordenacao == "Endereco" ? "Endereco_Desc" : "Endereco";

            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            switch (ordenacao)
            {
                case "Nome_Desc":
                    PessoasJuridicas = db.PessoasJuridicas.OrderByDescending(s => s.Nome);
                    break;
                case "Endereco":
                    PessoasJuridicas = db.PessoasJuridicas.OrderBy(s => s.Endereco);
                    break;
                case "Endereco_Desc":
                    PessoasJuridicas = db.PessoasJuridicas.OrderByDescending(s => s.Endereco);
                    break;
                default:
                    PessoasJuridicas = db.PessoasJuridicas.OrderBy(s => s.Nome);
                    break;
            }

            return View(PessoasJuridicas.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: PessoaJuridica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaJuridica = db.Pessoas.Find(id);
            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaJuridica);
        }

        // GET: PessoaJuridica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaJuridica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CadastroPessoaJuridicaViewModel pessoaJuridicaViewModel)
        {
            if (ModelState.IsValid)
            {

                var pessoaJuridica = new PessoaJuridica
                {
                    PessoaID = pessoaJuridicaViewModel.PessoaID,
                    Bairro = pessoaJuridicaViewModel.Bairro,
                    CEP = pessoaJuridicaViewModel.CEP,
                    Cidade = pessoaJuridicaViewModel.Cidade,
                    CNPJ = pessoaJuridicaViewModel.CNPJ,
                    Email = pessoaJuridicaViewModel.Email,
                    Endereco = pessoaJuridicaViewModel.Endereco,
                    Nome = pessoaJuridicaViewModel.Nome,
                    Telefone = pessoaJuridicaViewModel.Telefone,
                    UF = pessoaJuridicaViewModel.UF,
                    Foto = pessoaJuridicaViewModel.Foto
                    
                };
                db.Pessoas.Add(pessoaJuridica);
                db.SaveChanges();

                if (pessoaJuridica.Foto.ContentLength > 0)
                {
                    var nomeArquivo = pessoaJuridica.PessoaID.ToString() + ".jpg";
                    var caminho = Path.Combine(Server.MapPath("~/Content/Images"), nomeArquivo);
                    pessoaJuridica.Foto.SaveAs(caminho);
                }

                TempData["Mensagem"] = "Pessoa Juridica Cadastrada Com Sucesso!";
                return RedirectToAction("Index");
            }

            return View(pessoaJuridicaViewModel);
        }

        // GET: PessoaJuridica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaJuridica = db.PessoasJuridicas.Find(id);

            AtualizaPessoaJuridicaViewModel pessoaJuridicaVM =
            new AtualizaPessoaJuridicaViewModel()
            {
                PessoaID = pessoaJuridica.PessoaID,
                Bairro = pessoaJuridica.Bairro,
                CEP = pessoaJuridica.CEP,
                Cidade = pessoaJuridica.Cidade,
                CNPJ = pessoaJuridica.CNPJ,
                Email = pessoaJuridica.Email,
                Endereco = pessoaJuridica.Endereco,
                Nome = pessoaJuridica.Nome,
                Telefone = pessoaJuridica.Telefone,
                UF = pessoaJuridica.UF
            };

            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaJuridicaVM);
        }

        // POST: PessoaJuridica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AtualizaPessoaJuridicaViewModel pessoaJuridicaViewModel)
        {
            if (ModelState.IsValid)
            {

                var pessoaJuridica = new PessoaJuridica
                {
                    PessoaID = pessoaJuridicaViewModel.PessoaID,
                    Bairro = pessoaJuridicaViewModel.Bairro,
                    CEP = pessoaJuridicaViewModel.CEP,
                    Cidade = pessoaJuridicaViewModel.Cidade,
                    CNPJ = pessoaJuridicaViewModel.CNPJ,
                    Email = pessoaJuridicaViewModel.Email,
                    Endereco = pessoaJuridicaViewModel.Endereco,
                    Nome = pessoaJuridicaViewModel.Nome,
                    Telefone = pessoaJuridicaViewModel.Telefone,
                    UF = pessoaJuridicaViewModel.UF
                };

                db.Entry(pessoaJuridica).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Pessoa Juridica Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            return View(pessoaJuridicaViewModel);
        }

        // GET: PessoaJuridica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pessoaJuridica = db.Pessoas.Find(id);
            if (pessoaJuridica == null)
            {
                return HttpNotFound();
            }
            return View(pessoaJuridica);
        }

        // POST: PessoaJuridica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pessoaJuridica = db.Pessoas.Find(id);
            var nomeArquivo = pessoaJuridica.PessoaID.ToString() + ".jpg";
            var caminho = Path.Combine(Server.MapPath("~/Content/Images"), nomeArquivo);
            if (System.IO.File.Exists(caminho))
            {
                System.IO.File.Delete(caminho);
            }
            db.Pessoas.Remove(pessoaJuridica);
            db.SaveChanges();
            TempData["Mensagem"] = "Pessoa Juridica Excluida Com Sucesso!";
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
