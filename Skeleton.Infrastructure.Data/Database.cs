using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Dependency;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Mappers;
using System;
using System.Collections.Generic;
using System.Data;

namespace Skeleton.Infrastructure.Data
{
    public sealed class Database :
        DatabaseContext, IDatabase
    {
        public Database(
            ILogger logger,
            IDatabaseConfiguration configuration,
            IMetadataProvider metadataProvider)
            : base(logger, configuration, metadataProvider)
        {
        }

        public int Execute(ISqlCommand command)
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                return CreateTextCommand(command)
                    .ExecuteNonQuery();
            });
        }

        public object ExecuteScalar(ISqlCommand command)
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                var result = CreateTextCommand(command)
                    .ExecuteScalar();

                return result is DBNull
                    ? null
                    : result.ChangeType();
            });
        }

        public TValue ExecuteScalar<TValue>(ISqlCommand command)
        {
            var result = ExecuteScalar(command);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
        }

        public int ExecuteStoredProcedure(ISqlCommand command)
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                return CreateStoredProcedureCommand(command)
                    .ExecuteNonQuery();
            });
        }

        public IEnumerable<dynamic> Find(ISqlCommand command)
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                var reader = CreateTextCommand(command)
                    .ExecuteReader();

                return reader.Map();
            });
        }

        public IEnumerable<TPoco> Find<TPoco>(ISqlCommand command)
            where TPoco : class
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                var reader = CreateTextCommand(command)
                    .ExecuteReader();

                return MetadataProvider
                    .CreateMapper<TPoco>(reader)
                    .MapQuery();
            });
        }

        public TPoco FirstOrDefault<TPoco>(ISqlCommand command)
            where TPoco : class
        {
            return RetryPolicy.Execute(() =>
            {
                OpenConnection();

                var reader = CreateTextCommand(command)
                    .ExecuteReader(CommandBehavior.SingleRow);

                return MetadataProvider
                    .CreateMapper<TPoco>(reader)
                    .MapSingle();
            });
        }
    }
}