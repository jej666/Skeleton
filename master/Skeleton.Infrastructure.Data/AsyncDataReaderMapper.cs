using Skeleton.Abstraction.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    internal sealed class AsyncDataReaderMapper<TPoco> :
            DataReaderMapperBase<TPoco>
        where TPoco : class
    {
        internal AsyncDataReaderMapper(
            IMetadataProvider accessorCache,
            IDataReader dataReader)
            : base(accessorCache, dataReader)
        {
        }

        internal async Task<IEnumerable<TPoco>> MapQueryAsync()
        {
            if (!IsReadable)
                return new List<TPoco>();

            return await ReadAsync(() => SetMatchingValues())
                .ContinueWith(list => list.Result.ToList());
        }

        internal async Task<TPoco> MapSingleAsync()
        {
            return await MapQueryAsync()
                .ContinueWith(list => list.Result.FirstOrDefault());
        }
    }
}