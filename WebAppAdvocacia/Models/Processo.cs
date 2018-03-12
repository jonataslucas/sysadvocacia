using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class Processo
    {
        public int ProcessoID { get; set; }

        [Required(ErrorMessage = "Descrição do Processo é Obrigatório")]
        [Display(Name = "Descrição do processo")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Numero do Processo é Obrigatório")]
        [Display(Name = "Numero do processo")]
        public int NumeroProcesso { get; set; }

        [Required(ErrorMessage = "Data de Abertura é Obrigatório")]
        [Display(Name = "Data de Abertura")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAbertura { get; set; }

        [Required(ErrorMessage = "Data de Conclusão é Obrigatório")]
        [Display(Name = "Data de Conclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataConclusao { get; set; }

        [Required(ErrorMessage = "Situação do Processo é Obrigatório")]
        [Display(Name = "Situação")]
        public TipoSituacao Situacao { get; set; }

        [Required(ErrorMessage = "Pessoa é Obrigatório")]
        public int PessoaID { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        [Required(ErrorMessage = "Vara é Obrigatório")]
        public int VaraID { get; set; }
        public virtual Vara Vara { get; set; }

        public virtual ICollection<Audiencia> Audiencias { get; set; }
        public virtual ICollection<Custa> Custas { get; set; }

    }
}