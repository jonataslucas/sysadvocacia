using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class PessoaFisica : Pessoa
    {
       
        [Required(ErrorMessage = "Cpf é Obrigatório")]
        [StringLength(14, ErrorMessage = "No Máximo 11 caracteres")]
        [Display(Name = "Cpf")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Rg é Obrigatório")]
        [StringLength(10, ErrorMessage = "No Máximo 8 caracteres")]
        [Display(Name = "Rg")]
        public string RG { get; set; }

    }
}