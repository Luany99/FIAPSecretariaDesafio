using System.Text.RegularExpressions;

namespace FIAPSecretariaDesafio.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        protected Email() { }

        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco) ||
                !Regex.IsMatch(endereco, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("E-mail inválido.");

            Endereco = endereco.Trim().ToLowerInvariant(); 
        }

        public override string ToString() => Endereco;

        public override bool Equals(object obj)
        {
            return obj is Email other && Endereco == other.Endereco;
        }

        public override int GetHashCode() => Endereco.GetHashCode();
    }

}
