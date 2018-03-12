using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class PessoaJuridica : Pessoa
    {
  
        [Required(ErrorMessage = "Cnpj é Obrigatório")]
        [StringLength(18, ErrorMessage = "No Máximo 14 caracteres")]
        [Display(Name = "Cnpj")]
        public string CNPJ { get; set; }
        
    }
}