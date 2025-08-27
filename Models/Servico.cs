using System.ComponentModel.DataAnnotations;

namespace MinhaAPI.Models
{
    public class Servico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do serviço deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço do serviço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço do serviço deve ser maior que zero.")]
        public decimal Preco { get; set; } // Alterado para 'decimal' que é mais preciso para valores monetários.

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; } = string.Empty;
    }
}
