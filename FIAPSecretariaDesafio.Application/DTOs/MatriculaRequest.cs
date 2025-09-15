using System.ComponentModel.DataAnnotations;

namespace FIAPSecretariaDesafio.Application.DTOs
{
    public class MatriculaRequest
    {
        [Required(ErrorMessage = "ID do aluno é obrigatório")]
        public Guid AlunoId { get; set; }

        [Required(ErrorMessage = "ID da turma é obrigatório")]
        public Guid TurmaId { get; set; }
    }
}
