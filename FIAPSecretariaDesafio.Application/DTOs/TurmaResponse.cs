namespace FIAPSecretariaDesafio.Application.DTOs
{
    public class TurmaResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int TotalAlunos { get; set; } 
    }
}
