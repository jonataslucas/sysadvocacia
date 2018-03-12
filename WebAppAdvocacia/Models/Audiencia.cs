using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppAdvocacia.Models
{
    public class Audiencia
    {
        //public Audiencia()
        //{
        //    this.Processo = new Processo();
        //}

        public int AudienciaID { get; set; }

        [Required(ErrorMessage = "Data de Audiência é Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Parecer é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Máximo 200 caracteres")]
        [Display(Name = "Parecer")]
        public string Parecer { get; set; }

        [Required(ErrorMessage = "Processo é Obrigatório")]
        public int ProcessoID { get; set; }
        public virtual Processo Processo { get; set; }

    }
}