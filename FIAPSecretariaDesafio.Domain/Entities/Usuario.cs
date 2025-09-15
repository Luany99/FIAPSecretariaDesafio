using FIAPSecretariaDesafio.Domain.Enums;

namespace FIAPSecretariaDesafio.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public ERole Role { get; set; }
    }
}