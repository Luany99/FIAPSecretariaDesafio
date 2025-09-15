namespace FIAPSecretariaDesafio.Application.Interfaces
{
    public interface IAuthenticateService
    {
        public string Authenticate(string userName, string password);
    }
}
