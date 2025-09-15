namespace FIAPSecretariaDesafio.Domain.Entities
{
    public class Aluno
    {
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public ValueObjects.Cpf Cpf { get; private set; }
        public ValueObjects.Email Email { get; private set; }
        public ValueObjects.Senha Senha { get; private set; }
        public string SenhaHash => Senha.Hash;

        public ICollection<Matricula> Matriculas { get; private set; } = new List<Matricula>();

        protected Aluno() { }

        public Aluno(string nome, DateTime dataNascimento, ValueObjects.Cpf cpf, ValueObjects.Email email, ValueObjects.Senha senha)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length < 3)
                throw new ArgumentException("Nome deve conter pelo menos 3 caracteres.");

            Nome = nome;
            DataNascimento = dataNascimento;
            Cpf = cpf ?? throw new ArgumentNullException(nameof(cpf));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Senha = senha ?? throw new ArgumentNullException(nameof(senha));
            Id = Guid.NewGuid();
        }
    }
}
