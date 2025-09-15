using Dapper;
using FIAPSecretariaDesafio.Domain.ValueObjects;
using System.Data;

namespace FIAPSecretariaDesafio.Infrastructure.TypeHandlers
{
    public class EmailTypeHandler : SqlMapper.TypeHandler<Email>
    {
        public override void SetValue(IDbDataParameter parameter, Email value)
        {
            parameter.Value = value?.Endereco;
        }

        public override Email Parse(object value)
        {
            return value == null ? null : new Email(value.ToString());
        }
    }
}
