namespace Skeleton.Infrastructure.Data
{
    using Common;
    using Common.Extensions;
    using Common.Reflection;
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class DatabaseAsync : DatabaseBase, IDatabaseAsync
    {
        public DatabaseAsync(
            IDatabaseConfiguration configuration,
            ITypeAccessorCache typeAccessorCache,
            ILogger logger)
            : base(configuration, typeAccessorCache, logger)
        { }

        public async Task<int> ExecuteAsync(
            string query,
            IDictionary<string, object> parameters)
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand)CreateTextCommand(query, parameters);

                return await command.ExecuteNonQueryAsync()
                                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        public async Task<TValue> ExecuteScalarAsync<TValue>(
            string query,
            IDictionary<string, object> parameters)
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand)CreateTextCommand(query, parameters);
                var result = await command.ExecuteScalarAsync()
                                          .ConfigureAwait(false);

                return result is DBNull ?
                    default(TValue) :
                    result.ChangeType<TValue>();
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        public async Task<int> ExecuteStoredProcedureAsync(
            string procedureName,
            IDictionary<string, object> parameters)
        {
            parameters.ThrowIfNull(() => parameters);

            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand)CreateStoredProcedureCommand(
                    procedureName, parameters);

                return await command.ExecuteNonQueryAsync()
                                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TResult>> FindAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand)CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                                          .ConfigureAwait(false);

                return await CreateMapperAsync<TResult>().MapQueryAsync(reader);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        public async Task<TResult> FirstOrDefaultAsync<TResult>(
            string query,
            IDictionary<string, object> parameters)
            where TResult : class
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand)CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                                          .ConfigureAwait(false);

                return await CreateMapperAsync<TResult>().MapSingleAsync(reader);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw;
            }
        }

        private DataReaderMapperAsync<TResult> CreateMapperAsync<TResult>()
            where TResult : class
        {
            return new DataReaderMapperAsync<TResult>(TypeAccessorCache);
        }
    }
}