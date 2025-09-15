using FIAPSecretariaDesafio.Application.Services;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace FIAPSecretariaDesafio.Tests.Services
{
    public class AlunoServiceTests
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IValidator<Aluno> _validator;
        private readonly AlunoService _alunoService;

        public AlunoServiceTests()
        {
            _alunoRepository = Substitute.For<IAlunoRepository>();
            _validator = Substitute.For<IValidator<Aluno>>();
            _alunoService = new AlunoService(_alunoRepository, _validator);
        }

        private Aluno CriarAlunoFake()
        {
            return new Aluno(
                "Maria",
                new DateTime(1999, 5, 1),
                new Domain.ValueObjects.Cpf("95338442051"),
                new Domain.ValueObjects.Email("maria@email.com"),
                new Domain.ValueObjects.Senha("Senha@@123!")
            );
        }

        [Fact]
        public async Task ObterPorIdAsync_RetornaAluno()
        {
            var aluno = CriarAlunoFake();
            _alunoRepository.GetByIdAsync(aluno.Id).Returns(aluno);

            var result = await _alunoService.ObterPorIdAsync(aluno.Id);

            Assert.Equal(aluno, result);
        }

        [Fact]
        public async Task ListarAsync_RetornaListaDeAlunos()
        {
            var lista = new List<Aluno> { CriarAlunoFake() };
            _alunoRepository.GetAllAsync(1, 10).Returns(lista);

            var result = await _alunoService.ListarAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task CadastrarAsync_AlunoValido_AdicionaAluno()
        {
            var aluno = CriarAlunoFake();
            _validator.ValidateAsync(aluno, default).Returns(Task.FromResult(new ValidationResult()));
            _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero).Returns((Aluno)null);
            _alunoRepository.GetByEmailAsync(aluno.Email.Endereco).Returns((Aluno)null);

            await _alunoService.CadastrarAsync(aluno);

            await _alunoRepository.Received(1).AddAsync(aluno);
        }

        [Fact]
        public async Task CadastrarAsync_AlunoInvalido_LancaValidationException()
        {
            var aluno = CriarAlunoFake();
            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Nome", "Erro no nome")
            });
            _validator.ValidateAsync(aluno, default).Returns(Task.FromResult(validationResult));

            await Assert.ThrowsAsync<ValidationException>(() => _alunoService.CadastrarAsync(aluno));
        }

        [Fact]
        public async Task CadastrarAsync_CpfExistente_LancaArgumentException()
        {
            var aluno = CriarAlunoFake();
            _validator.ValidateAsync(aluno, default).Returns(Task.FromResult(new ValidationResult()));
            _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero).Returns(aluno);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _alunoService.CadastrarAsync(aluno));
            Assert.Equal("Já existe um aluno com este CPF.", ex.Message);
        }

        [Fact]
        public async Task CadastrarAsync_EmailExistente_LancaArgumentException()
        {
            var aluno = CriarAlunoFake();
            _validator.ValidateAsync(aluno, default).Returns(Task.FromResult(new ValidationResult()));
            _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero).Returns((Aluno)null);
            _alunoRepository.GetByEmailAsync(aluno.Email.Endereco).Returns(aluno);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _alunoService.CadastrarAsync(aluno));
            Assert.Equal("Já existe um aluno com este e-mail.", ex.Message);
        }

        [Fact]
        public async Task AtualizarAsync_AlunoValido_AtualizaAluno()
        {
            var aluno = CriarAlunoFake();
            _validator.ValidateAsync(aluno, default).Returns(Task.FromResult(new ValidationResult()));
            _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero).Returns((Aluno)null);
            _alunoRepository.GetByEmailAsync(aluno.Email.Endereco).Returns((Aluno)null);

            await _alunoService.AtualizarAsync(aluno);

            await _alunoRepository.Received(1).UpdateAsync(aluno);
        }

        [Fact]
        public async Task RemoverAsync_ChamaDelete()
        {
            var id = Guid.NewGuid();
            await _alunoService.RemoverAsync(id);
            await _alunoRepository.Received(1).DeleteAsync(id);
        }
    }
}
