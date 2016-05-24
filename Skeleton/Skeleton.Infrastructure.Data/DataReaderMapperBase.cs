﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;

namespace Skeleton.Infrastructure.Data
{
    internal abstract class DataReaderMapperBase<TResult> where TResult : class
    {
        private readonly ITypeAccessor _accessor;

        private readonly Func<IMemberAccessor, bool> _simplePropertiesCondition =
            x => x.MemberType.IsPrimitive ||
                 x.MemberType == typeof(decimal) ||
                 x.MemberType == typeof(string);

        private readonly IList<IMemberAccessor> _tableColumns;

        protected internal DataReaderMapperBase(ITypeAccessorCache accessorCache)
        {
            _accessor = accessorCache.Get<TResult>();
            _tableColumns = _accessor.GetDeclaredOnlyProperties()
                .Where(_simplePropertiesCondition)
                .ToList();
        }

        protected internal TResult SetMatchingValues(
            IDataRecord record,
            object[] values)
        {
            var instance = _accessor.CreateInstance<TResult>();

            _tableColumns.ForEach(column =>
            {
                for (var index = 0; index < values.Length; ++index)
                {
                    if (values[index] == null || values[index] is DBNull)
                        continue;

                    if (string.Equals(record.GetName(index), column.Name))
                    {
                        column.SetValue(instance, values[index]);
                    }
                }
            });

            return instance;
        }
    }
}