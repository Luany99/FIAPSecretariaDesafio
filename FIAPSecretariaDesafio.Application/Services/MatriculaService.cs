using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly IValidator<Matricula> _validator;

        public MatriculaService(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository,
            ITurmaRepository turmaRepository,
            IValidator<Matricula> validator)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<Matricula>> ListarPorTurmaAsync(Guid turmaId)
            => await _matriculaRepository.GetByTurmaIdAsync(turmaId);

        public async Task<IEnumerable<Matricula>> ListarPorAlunoAsync(Guid alunoId)
            => await _matriculaRepository.GetByAlunoIdAsync(alunoId);

        public async Task MatricularAlunoAsync(Guid alunoId, Guid turmaId)
        {
            var matricula = new Matricula(alunoId, turmaId);

            var resultado = await _validator.ValidateAsync(matricula);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            var aluno = await _alunoRepository.GetByIdAsync(alunoId);
            var turma = await _turmaRepository.GetByIdAsync(turmaId);

            if (aluno == null)
                throw new ArgumentException("Aluno não encontrado.");
            if (turma == null)
                throw new ArgumentException("Turma não encontrada.");

            if (await _matriculaRepository.ExistsAsync(alunoId, turmaId))
                throw new InvalidOperationException("Aluno já está matriculado nesta turma.");

            await _matriculaRepository.AddAsync(matricula);
        }

        public async Task CancelarMatriculaAsync(Guid matriculaId)
            => await _matriculaRepository.DeleteAsync(matriculaId);
    }
}
