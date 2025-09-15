using Dapper;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FIAPSecretariaDesafio.Infrastructure.Data;

namespace FIAPSecretariaDesafio.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly DapperContext _context;

        public MatriculaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Matricula?> GetByIdAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Matriculas WHERE Id=@Id";
            return await connection.QueryFirstOrDefaultAsync<Matricula>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Matricula>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Matriculas";
            return await connection.QueryAsync<Matricula>(sql);
        }

        public async Task<IEnumerable<Matricula>> GetByTurmaIdAsync(Guid turmaId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Matriculas WHERE TurmaId=@TurmaId";
            return await connection.QueryAsync<Matricula>(sql, new { TurmaId = turmaId });
        }

        public async Task<IEnumerable<Matricula>> GetByAlunoIdAsync(Guid alunoId)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Matriculas WHERE AlunoId=@AlunoId";
            return await connection.QueryAsync<Matricula>(sql, new { AlunoId = alunoId });
        }

        public async Task<bool> ExistsAsync(Guid alunoId, Guid turmaId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(1) FROM Matriculas WHERE AlunoId=@AlunoId AND TurmaId=@TurmaId";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { AlunoId = alunoId, TurmaId = turmaId });
            return count > 0;
        }

        public async Task AddAsync(Matricula matricula)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Matriculas (Id, AlunoId, TurmaId, DataMatricula) 
                        VALUES (@Id, @AlunoId, @TurmaId, @DataMatricula)";
            await connection.ExecuteAsync(sql, matricula);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = _context.CreateConnection();
            var sql = "DELETE FROM Matriculas WHERE Id=@Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
