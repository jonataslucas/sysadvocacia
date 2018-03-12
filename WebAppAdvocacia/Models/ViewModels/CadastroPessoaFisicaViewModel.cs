using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
namespace WebAppAdvocacia.Models.ViewModels
{
    public class CadastroPessoaFisicaViewModel
    {
        public int PessoaID { get; set; }
        [Required(ErrorMessage = "Nome Da Pessoa é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Maximo 200 Caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Endereço é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Maximo 200 Caracteres")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Telefone é Obrigatório")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Forneça o número do telefone no formato (000) 000-0000")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Cep é Obrigatório")]
        [RegularExpression(@"^\d{8}$|^\d{5}-\d{3}$", ErrorMessage = "O código postal deverá estar no formato 00000000 ou 00000-000")]
        [Display(Name = "Cep")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Bairro é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Maximo 200 Caracteres")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade é Obrigatório")]
        [StringLength(200, ErrorMessage = "No Maximo 200 Caracteres")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Uf é Obrigatório")]
        [StringLength(2, ErrorMessage = "No Maximo 2 Caracteres")]
        [Display(Name = "Uf")]
        public string UF { get; set; }

        [Required(ErrorMessage = "Email é Obrigatório")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um E-mail válido")]
        [StringLength(200, ErrorMessage = "No Maximo 200 Caracteres")]
        [Display(Name = "E-Mail")]
        [Remote("VerificaEmail", "PessoaFisica", ErrorMessage = "este E-Mail já existe.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cpf é Obrigatório")]
        [StringLength(14, ErrorMessage = "No Máximo 11 caracteres")]
        [Display(Name = "Cpf")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Rg é Obrigatório")]
        [StringLength(10, ErrorMessage = "No Máximo 8 caracteres")]
        [Display(Name = "Rg")]

        public string RG { get; set; }

        [NotMapped]
        [Display(Name = "Foto")]
        public HttpPostedFileBase Foto { get; set; }
    }
}