using FIAPSecretariaDesafio.Application.Services;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace FIAPSecretariaDesafio.Tests.Services
{
    public class TurmaServiceTests
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IValidator<Turma> _validator;
        private readonly TurmaService _turmaService;

        public TurmaServiceTests()
        {
            _turmaRepository = Substitute.For<ITurmaRepository>();
            _validator = Substitute.For<IValidator<Turma>>();
            _turmaService = new TurmaService(_turmaRepository, _validator);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarTurma_QuandoExistir()
        {
            var turma = new Turma("Matemática", "Turma de cálculo");
            _turmaRepository.GetByIdAsync(turma.Id).Returns(Task.FromResult<Turma?>(turma));

            var result = await _turmaService.ObterPorIdAsync(turma.Id);

            Assert.NotNull(result);
            Assert.Equal("Matemática", result.Nome);
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarTurmas()
        {
            var turmas = new List<Turma> { new Turma("Física", "Turma de física") };
            _turmaRepository.GetAllAsync(1, 10).Returns(Task.FromResult<IEnumerable<Turma>>(turmas));

            var result = await _turmaService.ListarAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task ContarAlunosAsync_DeveRetornarQuantidadeDeAlunos()
        {
            var turmaId = Guid.NewGuid();
            _turmaRepository.GetTotalAlunosAsync(turmaId).Returns(Task.FromResult(25));

            var result = await _turmaService.ContarAlunosAsync(turmaId);

            Assert.Equal(25, result);
        }

        [Fact]
        public async Task CadastrarAsync_DeveCadastrarTurma_QuandoValida()
        {
            var turma = new Turma("História", "Turma de história");
            _validator.ValidateAsync(turma).Returns(Task.FromResult(new ValidationResult()));

            await _turmaService.CadastrarAsync(turma);

            await _turmaRepository.Received(1).AddAsync(turma);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarTurma_QuandoValida()
        {
            var turma = new Turma("Geografia", "Turma de geografia");
            _validator.ValidateAsync(turma).Returns(Task.FromResult(new ValidationResult()));

            await _turmaService.AtualizarAsync(turma);

            await _turmaRepository.Received(1).UpdateAsync(turma);
        }

        [Fact]
        public async Task AtualizarAsync_DeveLancarExcecao_QuandoInvalida()
        {
            var turma = new Turma("Geo", "Nome inválido");
            var failures = new List<ValidationFailure> { new ValidationFailure("Nome", "Inválido") };
            _validator.ValidateAsync(turma).Returns(Task.FromResult(new ValidationResult(failures)));

            await Assert.ThrowsAsync<ValidationException>(() => _turmaService.AtualizarAsync(turma));
            await _turmaRepository.DidNotReceive().UpdateAsync(Arg.Any<Turma>());
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverTurma()
        {
            var turmaId = Guid.NewGuid();

            await _turmaService.RemoverAsync(turmaId);

            await _turmaRepository.Received(1).DeleteAsync(turmaId);
        }
    }
}
