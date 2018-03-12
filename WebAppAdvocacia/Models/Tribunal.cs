using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class Tribunal
    {
        public int TribunalID { get; set; }

        [Required(ErrorMessage = "Descrição é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Máximo 200 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Endereço é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Máximo 200 caracteres")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        public virtual ICollection<Vara> Varas { get; set; }

    }
}