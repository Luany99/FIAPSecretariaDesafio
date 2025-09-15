using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface IAlunoService
    {
        Task<Aluno?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Aluno>> ListarAsync(int page = 1, int pageSize = 10);
        Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome);
        Task CadastrarAsync(Aluno aluno);
        Task AtualizarAsync(Aluno aluno);
        Task RemoverAsync(Guid id);
        Task<Aluno?> GetByEmailAsync(string email);
    }
}
