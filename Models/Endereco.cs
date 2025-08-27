using ApiClientes.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Endereco
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Logradouro { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string Numero { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Complemento { get; set; }

    [Required]
    [StringLength(50)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [StringLength(9, MinimumLength = 8)] 
    public string CEP { get; set; } = string.Empty;

    // Chave estrangeira para o Cliente
    public int ClienteId { get; set; }

    [JsonIgnore]
    public Cliente? Cliente { get; set; }
}