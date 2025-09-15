using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FIAPSecretariaDesafio.Infrastructure.Data;
using Dapper;


namespace FIAPSecretariaDesafio.Infrastructure.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly DapperContext _context;

        public AlunoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Aluno?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT Id, Nome, DataNascimento, Cpf, Email, 
                       SenhaHash AS Senha
                       FROM Alunos WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Aluno>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Alunos 
                        ORDER BY Nome 
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            return await connection.QueryAsync<Aluno>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        }

        public async Task<Aluno?> GetByCpfAsync(string cpf)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Alunos WHERE Cpf = @Cpf";
            return await connection.QueryFirstOrDefaultAsync<Aluno>(sql, new { Cpf = cpf });
        }

        public async Task<Aluno?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Alunos WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<Aluno>(sql, new { Email = email });
        }

        public async Task<IEnumerable<Aluno>> SearchByNameAsync(string name)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Alunos WHERE Nome LIKE @Name ORDER BY Nome";
            return await connection.QueryAsync<Aluno>(sql, new { Name = $"%{name}%" });
        }

        public async Task AddAsync(Aluno aluno)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Alunos (Id, Nome, DataNascimento, Cpf, Email, SenhaHash) 
                VALUES (@Id, @Nome, @DataNascimento, @Cpf, @Email, @SenhaHash)";
            await connection.ExecuteAsync(sql, aluno);
        }

        public async Task UpdateAsync(Aluno aluno)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Alunos SET Nome=@Nome, DataNascimento=@DataNascimento, 
                        Cpf=@Cpf, Email=@Email, SenhaHash=@SenhaHash WHERE Id=@Id";
            await connection.ExecuteAsync(sql, aluno);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Alunos WHERE Id=@Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
