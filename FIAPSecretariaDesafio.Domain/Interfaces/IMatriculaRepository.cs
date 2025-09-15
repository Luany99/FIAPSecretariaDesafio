using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<Matricula?> GetByIdAsync(Guid id);
        Task<IEnumerable<Matricula>> GetAllAsync();
        Task<IEnumerable<Matricula>> GetByTurmaIdAsync(Guid turmaId);
        Task<IEnumerable<Matricula>> GetByAlunoIdAsync(Guid alunoId);
        Task<bool> ExistsAsync(Guid alunoId, Guid turmaId);
        Task AddAsync(Matricula matricula);
        Task DeleteAsync(Guid id);
    }
}
