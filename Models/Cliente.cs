using System.ComponentModel.DataAnnotations;

namespace ApiClientes.Models
{
    public class Cliente
    {
        // Identificação
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "O CPF do cliente é obrigatório.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve conter entre 11 e 14 caracteres.")]
        public string Cpf { get; set; } = string.Empty;


        [StringLength(15, MinimumLength = 7)]
        public string? RG { get; set; }


        // Contato
        [Required(ErrorMessage = "O email do cliente é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        [StringLength(100, ErrorMessage = "O email deve conter no máximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "O telefone do cliente é obrigatório.")]
        [Phone(ErrorMessage = "O formato do telefone é inválido.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "O telefone deve conter entre 10 e 15 caracteres.")]
        public string Telefone { get; set; } = string.Empty;

        
        // Dados Pessoais
        [Required(ErrorMessage = "A data de nascimento do cliente é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }


        // Relacionamento (Endereços)
        public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

        // Dados de Controle do Sistema
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}