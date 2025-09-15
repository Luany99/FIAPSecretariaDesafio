using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface ITurmaService
    {
        Task<Turma?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Turma>> ListarAsync(int page = 1, int pageSize = 10);
        Task<int> ContarAlunosAsync(Guid turmaId);
        Task CadastrarAsync(Turma turma);
        Task AtualizarAsync(Turma turma);
        Task RemoverAsync(Guid id);
    }
}
