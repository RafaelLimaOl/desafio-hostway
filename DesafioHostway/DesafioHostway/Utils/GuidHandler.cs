using Dapper;
using System.Data;

namespace DesafioHostway.Utils;

public class GuidHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value) => Guid.Parse(value.ToString()!);
    public override void SetValue(IDbDataParameter parameter, Guid value)
        => parameter.Value = value.ToString();
}
