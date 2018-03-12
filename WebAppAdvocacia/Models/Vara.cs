using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebAppAdvocacia.Models
{
    public class Vara
    {
        public int VaraID { get; set; }

        [Required(ErrorMessage = "Descrição da Vara é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Máximo 200 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Tribunal é Obrigatório")]
        public int TribunalID { get; set; }
        public virtual Tribunal Tribunal { get; set; }

        public virtual ICollection<Processo> Processos { get; set; }

    }
}