using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebAppAdvocacia.Models.ViewModels
{
    public class RelatorioGeralPessoasViewModel
    {
        [Display(Name = "PessoaID")]
        public int PessoaID { get; set; }

        [Display(Name = "PessoaNome")]
        public string PessoaNome { get; set; }

        [Display(Name = "PessoaBairro")]
        public string PessoaBairro { get; set; }

        [Display(Name = "PessoaEmail")]
        public string PessoaEmail { get; set; }

        [Display(Name = "ProcessoDescricao")]
        public string ProcessoDescricao { get; set; }

        [Display(Name = "ProcessoDataAbertura")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime ProcessoDataAbertura { get; set; }

        [Display(Name = "ProcessoDataConclusao")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime? ProcessoDataConclusao { get; set; }

        [Display(Name = "ProcessoSituacao")]
        public TipoSituacao ProcessoSituacao { get; set; }

        [Display(Name = "AudienciaData")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime AudienciaData { get; set; }

        [Display(Name = "AudienciaParecer")]
        public string AudienciaParecer { get; set; }
    }
}