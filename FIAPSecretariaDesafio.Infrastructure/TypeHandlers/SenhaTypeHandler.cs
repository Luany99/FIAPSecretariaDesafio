using Dapper;
using FIAPSecretariaDesafio.Domain.ValueObjects;
using System.Data;

namespace FIAPSecretariaDesafio.Infrastructure.TypeHandlers
{
    public class SenhaTypeHandler : SqlMapper.TypeHandler<Senha>
    {
        public override void SetValue(IDbDataParameter parameter, Senha value)
        {
            parameter.Value = value?.Hash;
        }

        public override Senha Parse(object value)
        {
            return value == null ? null : new Senha(value.ToString(), isHashed: true);
        }
    }
}
