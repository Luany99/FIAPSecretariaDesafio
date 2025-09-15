// Domain/ValueObjects/Senha.cs
using System.Text.RegularExpressions;

namespace FIAPSecretariaDesafio.Domain.ValueObjects
{
    public class Senha
    {
        public string Hash { get; private set; }

        protected Senha() { }

        // Construtor para senha em texto puro (criptografa)
        public Senha(string senha)
        {
            ValidarSenha(senha);
            Hash = BCrypt.Net.BCrypt.HashPassword(senha);
        }

        // ⭐ NOVO CONSTRUTOR para senha já hasheada ⭐
        public Senha(string hash, bool isHashed)
        {
            if (isHashed)
            {
                Hash = hash;
            }
            else
            {
                ValidarSenha(hash);
                Hash = BCrypt.Net.BCrypt.HashPassword(hash);
            }
        }

        private void ValidarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) ||
                senha.Length < 8 ||
                !Regex.IsMatch(senha, @"[A-Z]") ||
                !Regex.IsMatch(senha, @"[a-z]") ||
                !Regex.IsMatch(senha, @"[0-9]") ||
                !Regex.IsMatch(senha, @"[\W_]"))
            {
                throw new ArgumentException("Senha inválida. Deve conter ao menos 8 caracteres, incluindo maiúscula, minúscula, número e símbolo especial.");
            }
        }

        public bool Verificar(string senha) =>
            BCrypt.Net.BCrypt.Verify(senha, Hash);

        public override bool Equals(object obj)
        {
            return obj is Senha other && Hash == other.Hash;
        }

        public override int GetHashCode() => Hash.GetHashCode();
    }
}