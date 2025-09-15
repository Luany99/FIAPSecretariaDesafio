using Dapper;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Interfaces;
using FIAPSecretariaDesafio.Infrastructure.Data;

namespace FIAPSecretariaDesafio.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DapperContext _context;

        public UsuarioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Usuario WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public bool ExistUser(string username)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = "SELECT COUNT(*) FROM Usuario WHERE Username = @Username";
                var count = conn.QueryFirstOrDefault<int>(qry, new { Username = username });
                return count > 0;
            }
        }
        public bool ExistEmail(string email)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
                var count = conn.QueryFirstOrDefault<int>(qry, new { Email = email });
                return count > 0;
            }
        }

        public async Task CreateAsync(Usuario user)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = @"
                    INSERT INTO Usuario (Username, PasswordHash, Role, Email) 
                    VALUES (@Username, @PasswordHash, @Role, @Email); 
                    SELECT SCOPE_IDENTITY();";


                var insertedId = await conn.QueryFirstOrDefaultAsync<int>(qry, user);
                user.Id = insertedId;
            }
        }

        public async Task<Usuario> GetUserByName(string username)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = "SELECT * FROM Usuario WHERE Username = @Username";
                return await conn.QueryFirstOrDefaultAsync<Usuario>(qry, new { Username = username });
            }
        }

        public async Task UpdateAsync(Usuario user)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = @"
            UPDATE Usuario 
            SET Username = @Username, PasswordHash = @PasswordHash, Email = @Email, Role = @Role
            WHERE Id = @Id";

                await conn.ExecuteAsync(qry, user);
            }
        }

        public async Task DeleteAsync(int userId)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = "DELETE FROM Usuario WHERE Id = @Id";
                await conn.ExecuteAsync(qry, new { Id = userId });
            }
        }

        public async Task<Usuario> GetUserById(int userId)
        {
            using (var conn = _context.CreateConnection())
            {
                string qry = "SELECT * FROM Usuario WHERE Id = @Id";
                return await conn.QueryFirstOrDefaultAsync<Usuario>(qry, new { Id = userId });
            }
        }


    }
}