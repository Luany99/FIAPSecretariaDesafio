using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Validators
{
    public class TurmaValidator : AbstractValidator<Turma>
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaValidator(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;

            RuleFor(t => t.Nome)
                .NotEmpty().WithMessage("O nome da turma é obrigatório.")
                .MinimumLength(3).WithMessage("O nome da turma deve ter no mínimo 3 caracteres.");

            RuleFor(t => t.Descricao)
                .NotNull().WithMessage("A descrição da turma é obrigatória.");
        }
    }
}