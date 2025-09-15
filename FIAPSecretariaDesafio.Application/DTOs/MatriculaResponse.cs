namespace FIAPSecretariaDesafio.Application.DTOs
{
    public class MatriculaResponse
    {
        public Guid Id { get; set; }
        public Guid AlunoId { get; set; }
        public string AlunoNome { get; set; } = string.Empty;
        public Guid TurmaId { get; set; }
        public string TurmaNome { get; set; } = string.Empty;
        public DateTime DataMatricula { get; set; }
    }
}
