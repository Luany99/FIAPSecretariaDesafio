using FIAPSecretariaDesafio.Domain.Entities;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Validators
{
    public class MatriculaValidator : AbstractValidator<Matricula>
    {
        public MatriculaValidator()
        {
            RuleFor(m => m.AlunoId)
                .NotEmpty().WithMessage("O ID do aluno é obrigatório.");

            RuleFor(m => m.TurmaId)
                .NotEmpty().WithMessage("O ID da turma é obrigatório.");

            RuleFor(m => m.DataMatricula)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("A data da matrícula não pode ser futura.");
        }
    }
}