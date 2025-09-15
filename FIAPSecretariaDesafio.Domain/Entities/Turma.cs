namespace FIAPSecretariaDesafio.Domain.Entities
{
    public class Turma
    {
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();
        protected Turma() { }

        public Turma(string nome, string descricao)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("Nome da turma deve conter pelo menos 3 caracteres.");

            Nome = nome;
            Descricao = descricao;
            Id = Guid.NewGuid();
        }
    }
}
