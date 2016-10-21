using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;

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

        public int Execute(ISqlCommand sqlCommand)
        {
            return WrapRetryPolicy(() =>
                CreateTextCommand(sqlCommand)
                    .ExecuteNonQuery());
        }

        public object ExecuteScalar(ISqlCommand sqlCommand)
        {
            return WrapRetryPolicy(() =>
            {
                var result = CreateTextCommand(sqlCommand)
                    .ExecuteScalar();

                return result is DBNull
                    ? null
                    : result.ChangeType();
            });
        }

        public TValue ExecuteScalar<TValue>(ISqlCommand sqlCommand)
        {
            var result = ExecuteScalar(sqlCommand);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
        }

        public int ExecuteStoredProcedure(ISqlCommand procStockCommand)
        {
            return WrapRetryPolicy(() =>
                CreateStoredProcedureCommand(procStockCommand)
                    .ExecuteNonQuery());
        }

        public IEnumerable<dynamic> Find(ISqlCommand sqlCommand)
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(sqlCommand)
                    .ExecuteReader();

                return reader.Map();
            });
        }

        public IEnumerable<TPoco> Find<TPoco>(ISqlCommand sqlCommand)
            where TPoco : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(sqlCommand)
                    .ExecuteReader();

                return MetadataProvider.CreateMapper<TPoco>()
                    .MapQuery(reader);
            });
        }

        public TPoco FirstOrDefault<TPoco>(ISqlCommand sqlCommand)
            where TPoco : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(sqlCommand)
                    .ExecuteReader(CommandBehavior.SingleRow);

                return MetadataProvider.CreateMapper<TPoco>()
                    .MapSingle(reader);
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
                        throw new DataAccessException(e);

                    if ((e.Number != 1205) && (e.Number != -2))
                        throw new DataAccessException(e);

                    Thread.Sleep(delay);
                }
        }
    }
}