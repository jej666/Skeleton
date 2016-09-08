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

        public int Execute(
            string query,
            IDictionary<string, object> parameters)
        {
            return WrapRetryPolicy(() =>
                CreateTextCommand(query, parameters)
                    .ExecuteNonQuery());
        }

        public object ExecuteScalar(
            string query,
            IDictionary<string, object> parameters)
        {
            return WrapRetryPolicy(() =>
            {
                var result = CreateTextCommand(query, parameters)
                    .ExecuteScalar();

                return result is DBNull
                    ? null
                    : result.ChangeType();
            });
        }

        public TValue ExecuteScalar<TValue>(
            string query,
            IDictionary<string, object> parameters)
        {
            var result = ExecuteScalar(query, parameters);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
        }

        public int ExecuteStoredProcedure(
            string procedureName,
            IDictionary<string, object> parameters)
        {
            parameters.ThrowIfNull(() => parameters);

            return WrapRetryPolicy(() =>
                CreateStoredProcedureCommand(procedureName, parameters)
                    .ExecuteNonQuery());
        }

        public IEnumerable<dynamic> Find(
            string query,
            IDictionary<string, object> parameters)
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(query, parameters)
                    .ExecuteReader();

                return reader.Map();
            });
        }

        public IEnumerable<TPoco> Find<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(query, parameters)
                    .ExecuteReader();

                return MetadataProvider.CreateMapper<TPoco>()
                    .MapQuery(reader);
            });
        }

        public TPoco FirstOrDefault<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(query, parameters)
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