using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailAsync(string email);

        Task CreateAsync(Usuario user);
        bool ExistUser(string username);
        bool ExistEmail(string email);
        Task<Usuario> GetUserByName(string username);
        Task UpdateAsync(Usuario user);
        Task DeleteAsync(int userId);
        Task<Usuario> GetUserById(int userId);

    }
}