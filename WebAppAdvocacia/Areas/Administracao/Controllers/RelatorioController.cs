using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAdvocacia.Models.ViewModels;
using WebAppAdvocacia.Models;
namespace Areas.Administracao.Controllers
{
    public class RelatorioController : Controller
    {
        private ContextoEF db = new ContextoEF();
        // GET: Administracao/Relatorio
        public ActionResult RelatorioGeralPessoas()
        {
            var ConsultaPessoasFisicasSeusProcessosEsuasAudiencias = ConsultarPessoa();
            return View(ConsultaPessoasFisicasSeusProcessosEsuasAudiencias);
        }
        public IEnumerable<RelatorioGeralPessoasViewModel> ConsultarPessoa()
        {
            var ConsultaPessoasFisicasSeusProcessosEsuasAudiencias = (
                from p in db.Pessoas
                join pro in db.Processos
                on p.PessoaID equals pro.PessoaID
                join a in db.Audiencias
                on pro.ProcessoID equals a.ProcessoID
                where (p.Nome.ToUpper().Contains("Jonatas"))
                orderby p.Nome

                select new RelatorioGeralPessoasViewModel
                {
                    PessoaID = p.PessoaID,
                    PessoaNome = p.Nome,
                    PessoaBairro = p.Bairro,
                    PessoaEmail = p.Email,
                    ProcessoDescricao = pro.Descricao,
                    ProcessoDataAbertura = pro.DataAbertura,
                    ProcessoDataConclusao = pro.DataConclusao,
                    ProcessoSituacao = pro.Situacao,
                    AudienciaData = a.Data,
                    AudienciaParecer = a.Parecer
                }
              ).ToList();
            return ConsultaPessoasFisicasSeusProcessosEsuasAudiencias;
            

        }
    }
}