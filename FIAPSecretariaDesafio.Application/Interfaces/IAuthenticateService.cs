namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface IAuthenticateService
    {
        public string Authenticate(string email, string password);
    }
}
