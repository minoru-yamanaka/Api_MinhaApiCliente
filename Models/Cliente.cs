using System.ComponentModel.DataAnnotations;
// Namespace usado para anotações de validação, como [Required], [StringLength], [EmailAddress] etc.

namespace ApiClientes.Models
{
    public class Cliente
    {
        // Chave primária da tabela (Identity, auto incremento)
        public int Id { get; set; }

        // Nome é obrigatório, com limite de 100 caracteres
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve conter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // CPF obrigatório, entre 11 e 14 caracteres (pode incluir pontos e traço)
        [Required(ErrorMessage = "O CPF do cliente é obrigatório.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve conter entre 11 e 14 caracteres.")]
        public string Cpf { get; set; } = string.Empty;

        // RG é opcional, mas se informado precisa ter entre 7 e 15 caracteres
        [StringLength(15, MinimumLength = 7)]
        public string? RG { get; set; }

        // Email obrigatório, validado como formato de email e no máximo 100 caracteres
        [Required(ErrorMessage = "O email do cliente é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        [StringLength(100, ErrorMessage = "O email deve conter no máximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        // Telefone obrigatório, validado com formato de telefone e comprimento de 10 a 15 caracteres
        [Required(ErrorMessage = "O telefone do cliente é obrigatório.")]
        [Phone(ErrorMessage = "O formato do telefone é inválido.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "O telefone deve conter entre 10 e 15 caracteres.")]
        public string Telefone { get; set; } = string.Empty;

        // Data de nascimento obrigatória, validada como data
        [Required(ErrorMessage = "A data de nascimento do cliente é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        // Relacionamento 1:N → um cliente pode ter vários endereços
        // ICollection torna a navegação possível no EF Core
        public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

        // Metadados de controle (gerados pelo sistema)
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow; // Preenchido automaticamente ao criar
        public DateTime? DataUltimaAtualizacao { get; set; }          // Atualizado quando há alterações
        public bool Ativo { get; set; } = true;                       // Status de cliente ativo/inativo
    }
}

//-> Resumindo:

//Essa classe define regras de negócio e validações que já são aplicadas no momento do cadastro.

//O EF Core vai gerar a tabela Clientes no banco a partir desse modelo.

//O relacionamento com Endereco permite consultas como cliente.Enderecos.