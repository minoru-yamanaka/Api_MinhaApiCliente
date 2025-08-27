using System.ComponentModel.DataAnnotations;

namespace MinhaAPI.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço do produto deve ser maior que zero.")]
        public double Preco { get; set; }
        
        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        public int Estoque { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        public static implicit operator Produto?(Servico? v)
        {
            throw new NotImplementedException();
        }
    }
}
