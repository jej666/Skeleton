using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    internal abstract class DataReaderMapperBase<TPoco> where TPoco : class
    {
        private readonly IMetadata _accessor;
        private readonly IDataReader _reader;
        private readonly IList<IMemberAccessor> _tableColumns;
        private readonly IInstanceAccessor _instanceAccessor;

        protected internal DataReaderMapperBase(IMetadataProvider accessorCache, IDataReader reader)
        {
            _accessor = accessorCache.GetMetadata<TPoco>();
            _instanceAccessor = _accessor.GetConstructor();
            _tableColumns = _accessor.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToList();
            _reader = reader;
        }

        protected internal IDataReader DataReader
        {
            get { return _reader; }
        }

        protected internal IEnumerable<TPoco> Read(Func<TPoco> func)
        {
            try
            {
                while (DataReader.Read())
                {
                    yield return func();
                }

                while (DataReader.NextResult())
                {
                }
            }
            finally
            {
                using (DataReader)
                {
                }
            }
        }

        protected internal async Task<IEnumerable<TPoco>> ReadAsync(Func<TPoco> func)
        {
            try
            {
                var dbDataReader = (DbDataReader)DataReader;
                var list = new List<TPoco>();

                while (await dbDataReader.ReadAsync().ConfigureAwait(false))
                {
                    list.Add(func());
                }

                while (await dbDataReader.NextResultAsync().ConfigureAwait(false))
                {
                }
                return list;
            }
            finally
            {
                using (DataReader)
                {
                }
            }
        }

        internal bool IsReadable
        {
            get
            {
                return
                    DataReader != null &&
                    !DataReader.IsClosed;
            }
        }

        protected internal TPoco SetMatchingValues()
        {
            var values = new object[DataReader.FieldCount];
            var instance = _instanceAccessor.InstanceCreator(null) as TPoco;

            DataReader.GetValues(values);
            _tableColumns.ForEach(column =>
            {
                for (var index = 0; index < values.Length; ++index)
                {
                    if ((values[index] == null) || values[index] is DBNull)
                        continue;

                    if (string.Equals(DataReader.GetName(index), column.Name))
                        column.Setter(instance, values[index]);
                }
            });

            return instance;
        }
    }
}