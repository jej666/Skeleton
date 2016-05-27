using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Data
{
    [DebuggerDisplay("DatabaseName = {Configuration.Name")]
    public sealed class DatabaseAsync : DatabaseContext, IDatabaseAsync
    {
        public DatabaseAsync(
            IDatabaseConfiguration configuration,
            ITypeAccessorCache typeAccessorCache,
            ILogger logger)
            : base(configuration, typeAccessorCache, logger)
        {
        }

        public async Task<int> ExecuteAsync(
            string query,
            IDictionary<string, object> parameters)
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand) CreateTextCommand(query, parameters);

                return await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }

        public async Task<TValue> ExecuteScalarAsync<TValue>(
            string query,
            IDictionary<string, object> parameters)
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var result = await command.ExecuteScalarAsync()
                    .ConfigureAwait(false);

                return result is DBNull
                    ? default(TValue)
                    : result.ChangeType<TValue>();
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
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
                var command = (DbCommand) CreateStoredProcedureCommand(
                    procedureName, parameters);

                return await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
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
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await TypeAccessorCache.CreateMapperAsync<TResult>()
                    .MapQueryAsync(reader);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
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
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await TypeAccessorCache.CreateMapperAsync<TResult>()
                    .MapSingleAsync(reader);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }
    }
}