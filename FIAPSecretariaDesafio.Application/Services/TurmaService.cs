using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IValidator<Turma> _validator;


        public TurmaService(ITurmaRepository turmaRepository, IValidator<Turma> validator)
        {
            _turmaRepository = turmaRepository;
            _validator = validator;
        }

        public async Task<Turma?> ObterPorIdAsync(Guid id)
            => await _turmaRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Turma>> ListarAsync(int page = 1, int pageSize = 10)
            => await _turmaRepository.GetAllAsync(page, pageSize);

        public async Task<int> ContarAlunosAsync(Guid turmaId)
            => await _turmaRepository.GetTotalAlunosAsync(turmaId);

        public async Task CadastrarAsync(Turma turma)
        {
            var resultado = await _validator.ValidateAsync(turma);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            await _turmaRepository.AddAsync(turma);
        }

        public async Task AtualizarAsync(Turma turma)
        {
            var resultado = await _validator.ValidateAsync(turma);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            await _turmaRepository.UpdateAsync(turma);
        }

        public async Task RemoverAsync(Guid id)
            => await _turmaRepository.DeleteAsync(id);
    }
}
