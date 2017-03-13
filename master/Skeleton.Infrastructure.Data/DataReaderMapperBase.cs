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
        private readonly IMetadata _metadata;
        private readonly IDataReader _dataReader;
        private readonly IList<IMemberAccessor> _tableColumns;
        private readonly IInstanceAccessor _instanceAccessor;

        protected internal DataReaderMapperBase(IMetadataProvider metadataProvider, IDataReader dataReader)
        {
            _metadata = metadataProvider.GetMetadata<TPoco>();
            _instanceAccessor = _metadata.GetConstructor();
            _tableColumns = _metadata.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToList();
            _dataReader = dataReader;
        }

        protected internal IEnumerable<TPoco> Read(Func<TPoco> func)
        {
            try
            {
                while (_dataReader.Read())
                {
                    yield return func();
                }

                while (_dataReader.NextResult())
                {
                }
            }
            finally
            {
                using (_dataReader)
                {
                }
            }
        }

        protected internal async Task<IEnumerable<TPoco>> ReadAsync(Func<TPoco> func)
        {
            try
            {
                var dbDataReader = (DbDataReader)_dataReader;
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
                using (_dataReader)
                {
                }
            }
        }

        internal bool IsReadable
        {
            get
            {
                return
                    _dataReader != null &&
                    !_dataReader.IsClosed;
            }
        }

        protected internal TPoco SetMatchingValues()
        {
            var values = new object[_dataReader.FieldCount];
            var instance = _instanceAccessor.InstanceCreator(null) as TPoco;

            _dataReader.GetValues(values);
            _tableColumns.ForEach(column =>
            {
                for (var index = 0; index < values.Length; ++index)
                {
                    if ((values[index] == null) || values[index] is DBNull)
                        continue;

                    if (string.Equals(_dataReader.GetName(index), column.Name))
                        column.Setter(instance, values[index]);
                }
            });

            return instance;
        }
    }
}