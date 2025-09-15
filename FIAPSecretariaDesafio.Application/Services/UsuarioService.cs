using FIAPSecretariaDesafio.Application.DTOs;
using FIAPSecretariaDesafio.Application.Interfaces;
using FIAPSecretariaDesafio.Domain.Entities;
using FIAPSecretariaDesafio.Domain.Enums;
using FIAPSecretariaDesafio.Domain.Interfaces;
using System.Data;

namespace FIAPSecretariaDesafio.Application.Services
{
    public class UsuarioService :IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Usuario?> GetByEmailAsync(string email)
            => await _usuarioRepository.GetByEmailAsync(email);


        public void Exist(UsuarioDTO register)
        {
            if (_usuarioRepository.ExistUser(register.Username))
                throw new InvalidOperationException("Este usuário já foi cadastrado em sistema.");

            if (_usuarioRepository.ExistEmail(register.Email))
                throw new InvalidOperationException("Este endereço de Email já foi cadastrado em sistema.");
        }


        public async Task<Usuario> GetUserByName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("O nome de usuário não pode ser vazio ou nulo.");
            }

            var user = await _usuarioRepository.GetUserByName(username);
            return user;
        }


        public async Task Register(UsuarioDTO register)
        {
            Exist(register);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.PasswordHash);

            var user = new Usuario
            {
                Username = register.Username,
                PasswordHash = hashedPassword,
                Role = (ERole)register.Role,
                Email = register.Email,
            };
            await _usuarioRepository.CreateAsync(user);
        }

        public async Task EditUser(int id, UsuarioDTO editModel)
        {
            var user = await _usuarioRepository.GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(editModel.PasswordHash);

            user.Username = editModel.Username;
            user.PasswordHash = hashedPassword;
            user.Role = (ERole)editModel.Role;
            user.Email = editModel.Email;

            await _usuarioRepository.UpdateAsync(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _usuarioRepository.GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            await _usuarioRepository.DeleteAsync(user.Id);
        }

        public async Task<Usuario> GetUserById(int userId)
        {
            var user = await _usuarioRepository.GetUserById(userId);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            return user;
        }



    }
}