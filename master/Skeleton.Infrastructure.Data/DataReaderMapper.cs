using System.Collections.Generic;
using System.Data;
using System.Linq;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class DataReaderMapper<TPoco> :
            DataReaderMapperBase<TPoco>
        where TPoco : class
    {
        internal DataReaderMapper(
            IMetadataProvider accessorCache, 
            IDataReader dataReader)
            : base(accessorCache, dataReader)
        {
        }

        internal IEnumerable<TPoco> MapQuery()
        {
            if (!IsReadable)
                return new List<TPoco>();

            return Read(() => SetMatchingValues()).ToList();
        }

        internal TPoco MapSingle()
        {
            return MapQuery().FirstOrDefault();
        }
    }
}