using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MinhaAPI.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Logradouro é obrigatório")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O Bairro é obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O Cidade é obrigatório")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O Estado é obrigatório")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O CEP deve ter 9 caracteres (XXXXX-XXX)")]
        public string CEP { get; set; }


        [Required(ErrorMessage = "O Numero é obrigatório")]
        public string Numero { get; set; }

        public string? TipoEndereco { get; set; }

        public string? Complemento { get; set; }
        
        public int ClienteId { get; set; } // Chave estrangeira para Cliente

        [JsonIgnore]
        public Cliente? Cliente { get; set; } // Navegação para Cliente
        
    }
}
