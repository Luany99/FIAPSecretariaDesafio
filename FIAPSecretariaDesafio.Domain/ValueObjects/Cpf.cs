namespace FIAPSecretariaDesafio.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            numero = numero?.Trim().Replace(".", "").Replace("-", "");

            if (string.IsNullOrWhiteSpace(numero) || numero.Length != 11 || !ValidarCpf(numero))
                throw new ArgumentException("CPF inválido.");

            Numero = numero;
        }

        private bool ValidarCpf(string cpf)
        {
            var numerosInvalidos = new[]
            {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };

            if (numerosInvalidos.Contains(cpf))
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public override string ToString() => Numero;

        public override bool Equals(object obj)
        {
            return obj is Cpf other && Numero == other.Numero;
        }

        public override int GetHashCode() => Numero.GetHashCode();
    }

}
