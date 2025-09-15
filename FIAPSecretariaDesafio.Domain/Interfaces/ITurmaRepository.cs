using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Domain.Interfaces
{
    public interface ITurmaRepository
    {
        Task<Turma?> GetByIdAsync(Guid id);
        Task<IEnumerable<Turma>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<int> GetTotalAlunosAsync(Guid turmaId);

        Task AddAsync(Turma turma);
        Task UpdateAsync(Turma turma);
        Task DeleteAsync(Guid id);
    }
}
