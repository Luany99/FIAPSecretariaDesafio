namespace FIAPSecretariaDesafio.Domain.Entities
{
    public class Matricula
    {
        public Guid Id { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid TurmaId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public Aluno? Aluno { get; private set; }
        public Turma? Turma { get; private set; }

        protected Matricula() { } 

        public Matricula(Guid alunoId, Guid turmaId)
        {
            Id = Guid.NewGuid();
            AlunoId = alunoId;
            TurmaId = turmaId;
            DataMatricula = DateTime.UtcNow;
        }

        public void VincularAluno(Aluno aluno) => Aluno = aluno;
        public void VincularTurma(Turma turma) => Turma = turma;
    }
}
