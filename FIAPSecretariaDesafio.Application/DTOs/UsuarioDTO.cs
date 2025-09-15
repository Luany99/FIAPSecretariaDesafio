namespace FIAPSecretariaDesafio.Application.DTOs
{
    public class UsuarioDTO
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
