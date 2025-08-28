using ApiClientes.Models;               // Importa o modelo Cliente (relacionamento)
using System.ComponentModel.DataAnnotations; // Usado para validações com atributos (Required, StringLength etc.)
using System.Text.Json.Serialization;   // Usado para ignorar propriedades no retorno JSON

public class Endereco
{
    // Identificador único do endereço (chave primária)
    public int Id { get; set; }

    // Logradouro é obrigatório e pode ter no máximo 100 caracteres
    [Required]
    [StringLength(100)]
    public string Logradouro { get; set; } = string.Empty;

    // Número é obrigatório e limitado a 10 caracteres (ex.: "123", "123A")
    [Required]
    [StringLength(10)]
    public string Numero { get; set; } = string.Empty;

    // Complemento é opcional, no máximo 50 caracteres (ex.: "Apto 202", "Fundos")
    [StringLength(50)]
    public string? Complemento { get; set; }

    // Bairro obrigatório, até 50 caracteres
    [Required]
    [StringLength(50)]
    public string Bairro { get; set; } = string.Empty;

    // Cidade obrigatória, até 50 caracteres
    [Required]
    [StringLength(50)]
    public string Cidade { get; set; } = string.Empty;

    // Estado obrigatório, limitado a exatamente 2 caracteres (UF → ex.: SP, RJ)
    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string Estado { get; set; } = string.Empty;

    // CEP obrigatório, entre 8 e 9 caracteres (aceita com ou sem hífen → "12345678" ou "12345-678")
    [Required]
    [StringLength(9, MinimumLength = 8)]
    public string CEP { get; set; } = string.Empty;

    // Chave estrangeira para relacionar endereço ao cliente
    public int ClienteId { get; set; }

    // Propriedade de navegação para acessar os dados do Cliente dono do endereço
    // [JsonIgnore] evita serialização circular ao retornar Cliente → Enderecos → Cliente
    [JsonIgnore]
    public Cliente? Cliente { get; set; }
}

//-> Em resumo:

//Essa classe representa a tabela Enderecos.

//Tem validações automáticas via data annotations (garante consistência dos dados).

//O [JsonIgnore] é essencial para evitar loop infinito na serialização, já que Cliente tem lista de Enderecos e vice-versa.