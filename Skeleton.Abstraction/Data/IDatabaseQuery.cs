using System.Collections.Generic;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseQuery
    {
        IEnumerable<dynamic> Find(ISqlCommand command);

        IEnumerable<TPoco> Find<TPoco>(ISqlCommand command)
            where TPoco : class;

        TPoco FirstOrDefault<TPoco>(ISqlCommand command)
            where TPoco : class;
    }
}