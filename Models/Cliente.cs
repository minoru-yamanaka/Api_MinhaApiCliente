// Models/Cliente.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MinhaAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres")]
        public string SobreNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 caracteres")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "O telefone deve ter entre 10 e 15 caracteres")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;

        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }
}

//using System.ComponentModel.DataAnnotations;

//namespace MinhaAPI.Models
//{
//    public class Cliente
//    {
//        public int Id { get; set; }

//        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
//        [Required(ErrorMessage = "O nome é obrigatório")]
//        public string Nome { get; set; } = string.Empty;

//        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
//        [Required(ErrorMessage = "O nome é obrigatório")]
//        public string SobreNome { get; set; } = string.Empty;


//        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
//        [Required(ErrorMessage = "O email é obrigatório")]
//        public string Email { get; set; } = string.Empty;

//        [StringLength(14, MinimumLength = 11, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
//        [Required(ErrorMessage = "O CPF é obrigatório")]
//        public string Cpf { get; set; } = string.Empty;

//        [StringLength(14, MinimumLength = 11, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
//        [Required(ErrorMessage = "O telefone é obrigatório")]
//        public string Telefone { get; set; } = string.Empty;

//        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();


//    }
//}
