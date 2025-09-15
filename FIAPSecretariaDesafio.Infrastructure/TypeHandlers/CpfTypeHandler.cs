using Dapper;
using FIAPSecretariaDesafio.Domain.ValueObjects;
using System.Data;

namespace FIAPSecretariaDesafio.Infrastructure.TypeHandlers
{
    public class CpfTypeHandler : SqlMapper.TypeHandler<Cpf>
    {
        public override void SetValue(IDbDataParameter parameter, Cpf value)
        {
            parameter.Value = value?.Numero;
        }

        public override Cpf Parse(object value)
        {
            return value == null ? null : new Cpf(value.ToString());
        }
    }
}
