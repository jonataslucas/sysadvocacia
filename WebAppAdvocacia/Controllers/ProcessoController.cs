using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAdvocacia.Models;
using PagedList;
using Rotativa;
namespace WebAppAdvocacia.Controllers
{
    public class ProcessoController : Controller
    {
        
        private ContextoEF db = new ContextoEF();

        public ActionResult GerarPDF(int? pagina)
        {
            int tamanhoPagina = 2;
            int numeroPagina = pagina ?? 1;
            var modelo = db.Processos.OrderBy(e=>e.Situacao).ToPagedList(numeroPagina,tamanhoPagina);

            var pdf = new ViewAsPdf
            {
                ViewName = "IndexParaImpressao",
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
    }
}