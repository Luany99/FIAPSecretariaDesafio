using FIAPSecretariaDesafio.Domain.Entities;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Validators
{
    public class AlunoValidator : AbstractValidator<Aluno>
    {
        public AlunoValidator()
        {
            RuleFor(a => a.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres.");

            RuleFor(a => a.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .Must(d => d <= DateTime.UtcNow)
                .WithMessage("A data de nascimento não pode ser futura.");

            RuleFor(a => a.Cpf)
                .NotNull().WithMessage("O CPF é obrigatório.");

            RuleFor(a => a.Email)
                .NotNull().WithMessage("O e-mail é obrigatório.");

            RuleFor(a => a.Senha)
                .NotNull().WithMessage("A senha é obrigatória.");
        }
    }
}
