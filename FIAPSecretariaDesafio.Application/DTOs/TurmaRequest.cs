using System.ComponentModel.DataAnnotations;

namespace FIAPSecretariaDesafio.Application.DTOs
{
    public class TurmaRequest
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;
    }
}
