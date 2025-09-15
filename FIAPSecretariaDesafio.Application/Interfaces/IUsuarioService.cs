using FIAPSecretariaDesafio.Application.DTOs;
using FIAPSecretariaDesafio.Domain.Entities;

namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task Register(UsuarioDTO register);
        Task<Usuario> GetUserByName(string username);
        Task EditUser(int id, UsuarioDTO editModel);
        Task DeleteUser(int id);
        Task<Usuario> GetUserById(int userId);
    }
}
