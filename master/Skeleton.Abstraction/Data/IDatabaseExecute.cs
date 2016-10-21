using System.Collections.Generic;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseExecute
    {
        int Execute(ISqlCommand command);

        object ExecuteScalar(ISqlCommand command);

        TValue ExecuteScalar<TValue>(ISqlCommand command);

        int ExecuteStoredProcedure(ISqlCommand procStockCommand);
    }
}