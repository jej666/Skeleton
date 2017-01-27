using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Skeleton.Infrastructure.Data
{
    public sealed class Database : DatabaseContext, IDatabase
    {
        public Database(
            ILogger logger,
            IDatabaseConfiguration configuration,
            IMetadataProvider metadataProvider)
            : base(logger, configuration, metadataProvider)
        {
            OpenConnection();
        }

        public int Execute(ISqlCommand command)
        {
            return WrapRetryPolicy(() =>
                CreateTextCommand(command)
                    .ExecuteNonQuery());
        }

        public object ExecuteScalar(ISqlCommand command)
        {
            return WrapRetryPolicy(() =>
            {
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
            return WrapRetryPolicy(() =>
                CreateStoredProcedureCommand(command)
                    .ExecuteNonQuery());
        }

        public IEnumerable<dynamic> Find(ISqlCommand command)
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(command)
                    .ExecuteReader();

                return reader.Map();
            });
        }

        public IEnumerable<TPoco> Find<TPoco>(ISqlCommand command)
            where TPoco : class
        {
            return WrapRetryPolicy(() =>
            {
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
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(command)
                    .ExecuteReader(CommandBehavior.SingleRow);

                return MetadataProvider
                    .CreateMapper<TPoco>(reader)
                    .MapSingle();
            });
        }

        private T WrapRetryPolicy<T>(Func<T> func)
        {
            var retryCount = Configuration.RetryPolicyCount;
            var retryInterval = Configuration.RetryPolicyInterval;
            var delay = TimeSpan.FromSeconds(retryInterval);

            while (true)
                try
                {
                    return func();
                }
                catch (SqlException e)
                {
                    --retryCount;

                    Logger.Error("Database error => ", e);

                    if (retryCount <= 0)
                        throw;

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw;

                    Thread.Sleep(delay);
                }
        }
    }
}