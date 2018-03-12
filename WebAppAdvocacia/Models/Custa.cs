using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class Custa
    {
        public int CustaID { get; set; }

        [Required(ErrorMessage = "Data de Custa é Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Descrição é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Máximo 200 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Custa é Obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Processo é Obrigatório")]
        public int ProcessoID { get; set; }
        public virtual Processo Processo { get; set; }


    }
}