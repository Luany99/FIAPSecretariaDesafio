using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FluentValidation;

namespace FIAPSecretariaDesafio.Application.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IValidator<Aluno> _validator;

        public AlunoService(IAlunoRepository alunoRepository, IValidator<Aluno> validator)
        {
            _alunoRepository = alunoRepository;
            _validator = validator;
        }

        public async Task<Aluno?> ObterPorIdAsync(Guid id)
            => await _alunoRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Aluno>> ListarAsync(int page = 1, int pageSize = 10)
            => await _alunoRepository.GetAllAsync(page, pageSize);

        public async Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome)
            => await _alunoRepository.SearchByNameAsync(nome);

        public async Task CadastrarAsync(Aluno aluno)
        {
            var resultado = await _validator.ValidateAsync(aluno);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            if (await _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero) != null)
                throw new ArgumentException("Já existe um aluno com este CPF.");

            if (await _alunoRepository.GetByEmailAsync(aluno.Email.Endereco) != null)
                throw new ArgumentException("Já existe um aluno com este e-mail.");

            await _alunoRepository.AddAsync(aluno);
        }

        public async Task AtualizarAsync(Aluno aluno)
        {
            var resultado = await _validator.ValidateAsync(aluno);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            var existenteCpf = await _alunoRepository.GetByCpfAsync(aluno.Cpf.Numero);
            if (existenteCpf != null && existenteCpf.Id != aluno.Id)
                throw new ArgumentException("Já existe um aluno com este CPF.");

            var existenteEmail = await _alunoRepository.GetByEmailAsync(aluno.Email.Endereco);
            if (existenteEmail != null && existenteEmail.Id != aluno.Id)
                throw new ArgumentException("Já existe um aluno com este e-mail.");

            await _alunoRepository.UpdateAsync(aluno);
        }

        public async Task RemoverAsync(Guid id)
            => await _alunoRepository.DeleteAsync(id);

        public async Task<Aluno?> GetByEmailAsync(string email)
            => await _alunoRepository.GetByEmailAsync(email);
    }
}
