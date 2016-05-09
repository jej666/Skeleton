namespace Skeleton.Infrastructure.Data
{
    using Common;
    using Common.Extensions;
    using Common.Reflection;
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    public class Database : DatabaseBase, IDatabase
    {
        public Database(
            IDatabaseConfiguration configuration,
            ITypeAccessorCache typeAccessorCache,
            ILogger logger)
            : base(configuration, typeAccessorCache, logger)
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

        public TValue ExecuteScalar<TValue>(
            string query,
            IDictionary<string, object> parameters)
        {
            return WrapRetryPolicy(() =>
            {
                var result = CreateTextCommand(query, parameters)
                    .ExecuteScalar();

                return result is DBNull ?
                    default(TValue) :
                    result.ChangeType<TValue>();
            });
        }

        public int ExecuteStoredProcedure(
            string procedureName,
            IDictionary<string, object> parameters)
        {
            parameters.ThrowIfNull(() => parameters);

            return WrapRetryPolicy(() =>
            {
                return CreateStoredProcedureCommand(procedureName, parameters)
                    .ExecuteNonQuery();
            });
        }

        public IEnumerable<TResult> Find<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(query, parameters)
                    .ExecuteReader();

                return CreateMapper<TResult>().MapQuery(reader);
            });
        }

        public TResult FirstOrDefault<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class
        {
            return WrapRetryPolicy(() =>
            {
                var reader = CreateTextCommand(query, parameters)
                    .ExecuteReader(CommandBehavior.SingleRow);

                return CreateMapper<TResult>().MapSingle(reader);
            });
        }

        private DataReaderMapper<TResult> CreateMapper<TResult>()
            where TResult : class
        {
            return new DataReaderMapper<TResult>(TypeAccessorCache);
        }

        private TResult WrapRetryPolicy<TResult>(Func<TResult> func)
        {
            var retryCount = Configuration.RetryPolicyCount;
            var retryInterval = Configuration.RetryPolicyInterval;
            var delay = TimeSpan.FromSeconds(retryInterval);

            while (true)
            {
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

                    if (e.Number != 1205 && e.Number != -2)
                        throw new DataAccessException(e);

                    Thread.Sleep(delay);
                }
            }
        }

        //private TResult WrapProfiler<TResult>(Func<TResult> func)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    var res = func();
        //    stopWatch.Stop();

        //    double elapsed = stopWatch.Elapsed.TotalSeconds;

        //    System.Diagnostics.Debug.Write(
        //       elapsed + " second(s)");
        //    return res;
        //}
    }
}