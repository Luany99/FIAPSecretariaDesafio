using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface IMatriculaService
    {
        Task<IEnumerable<Matricula>> ListarPorTurmaAsync(Guid turmaId);
        Task<IEnumerable<Matricula>> ListarPorAlunoAsync(Guid alunoId);
        Task MatricularAlunoAsync(Guid alunoId, Guid turmaId);
        Task CancelarMatriculaAsync(Guid matriculaId);
    }
}
