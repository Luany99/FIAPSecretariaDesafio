using FIAPSecretariaDesafio.Application.Services;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace FIAPSecretariaDesafio.Tests.Services
{
    public class MatriculaServiceTests
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly IValidator<Matricula> _validator;
        private readonly MatriculaService _service;

        public MatriculaServiceTests()
        {
            _matriculaRepository = Substitute.For<IMatriculaRepository>();
            _alunoRepository = Substitute.For<IAlunoRepository>();
            _turmaRepository = Substitute.For<ITurmaRepository>();
            _validator = Substitute.For<IValidator<Matricula>>();

            _service = new MatriculaService(_matriculaRepository, _alunoRepository, _turmaRepository, _validator);
        }

        private Matricula CriarMatriculaFake()
        {
            return new Matricula(Guid.NewGuid(), Guid.NewGuid());
        }

        [Fact]
        public async Task ListarPorTurmaAsync_RetornaLista()
        {
            var turmaId = Guid.NewGuid();
            var lista = new List<Matricula> { CriarMatriculaFake() };
            _matriculaRepository.GetByTurmaIdAsync(turmaId).Returns(lista);

            var result = await _service.ListarPorTurmaAsync(turmaId);

            Assert.Single(result);
        }

        [Fact]
        public async Task ListarPorAlunoAsync_RetornaLista()
        {
            var alunoId = Guid.NewGuid();
            var lista = new List<Matricula> { CriarMatriculaFake() };
            _matriculaRepository.GetByAlunoIdAsync(alunoId).Returns(lista);

            var result = await _service.ListarPorAlunoAsync(alunoId);

            Assert.Single(result);
        }

        [Fact]
        public async Task MatricularAlunoAsync_MatriculaInvalida_LancaValidationException()
        {
            var alunoId = Guid.NewGuid();
            var turmaId = Guid.NewGuid();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("AlunoId", "Erro")
            });
            _validator.ValidateAsync(Arg.Any<Matricula>(), default)
                .Returns(Task.FromResult(validationResult));

            await Assert.ThrowsAsync<ValidationException>(() => _service.MatricularAlunoAsync(alunoId, turmaId));
        }

        [Fact]
        public async Task MatricularAlunoAsync_AlunoNaoEncontrado_LancaArgumentException()
        {
            var alunoId = Guid.NewGuid();
            var turmaId = Guid.NewGuid();
            _validator.ValidateAsync(Arg.Any<Matricula>(), default).Returns(Task.FromResult(new ValidationResult()));
            _alunoRepository.GetByIdAsync(alunoId).Returns((Aluno)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.MatricularAlunoAsync(alunoId, turmaId));
            Assert.Equal("Aluno não encontrado.", ex.Message);
        }

        [Fact]
        public async Task CancelarMatriculaAsync_ChamaDelete()
        {
            var matriculaId = Guid.NewGuid();
            await _service.CancelarMatriculaAsync(matriculaId);

            await _matriculaRepository.Received(1).DeleteAsync(matriculaId);
        }
    }
}
