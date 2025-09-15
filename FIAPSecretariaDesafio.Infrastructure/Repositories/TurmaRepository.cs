using Dapper;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FIAPSecretariaDesafio.Infrastructure.Data;

namespace FIAPSecretariaDesafio.Infrastructure.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly DapperContext _context;

        public TurmaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Turma?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Turmas WHERE Id=@Id";
            return await connection.QueryFirstOrDefaultAsync<Turma>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Turma>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Turmas 
                        ORDER BY Nome 
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            return await connection.QueryAsync<Turma>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
        }

        public async Task<int> GetTotalAlunosAsync(Guid turmaId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Matriculas WHERE TurmaId=@TurmaId";
            return await connection.ExecuteScalarAsync<int>(sql, new { TurmaId = turmaId });
        }

        public async Task AddAsync(Turma turma)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Turmas (Id, Nome, Descricao) 
                        VALUES (@Id, @Nome, @Descricao)";
            await connection.ExecuteAsync(sql, turma);
        }

        public async Task UpdateAsync(Turma turma)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Turmas SET Nome=@Nome, Descricao=@Descricao WHERE Id=@Id";
            await connection.ExecuteAsync(sql, turma);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Turmas WHERE Id=@Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
