using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Domain.Interfaces
{
    public interface IAlunoRepository
    {
        Task<Aluno?> GetByIdAsync(Guid id);
        Task<IEnumerable<Aluno>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<Aluno?> GetByCpfAsync(string cpf);
        Task<Aluno?> GetByEmailAsync(string email);
        Task<IEnumerable<Aluno>> SearchByNameAsync(string name);

        Task AddAsync(Aluno aluno);
        Task UpdateAsync(Aluno aluno);
        Task DeleteAsync(Guid id);
    }
}
