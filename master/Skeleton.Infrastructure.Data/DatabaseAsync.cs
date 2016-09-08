using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseAsync : DatabaseContext, IDatabaseAsync
    {
        public DatabaseAsync(
            ILogger logger,
            IDatabaseConfiguration configuration,
            IMetadataProvider metadataProvider)
            : base(logger, configuration, metadataProvider)
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


        public async Task<object> ExecuteScalarAsync(
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
                    ? null
                    : result;
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
            var result = await ExecuteScalarAsync(query, parameters);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
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

        public async Task<IEnumerable<dynamic>> FindAsync(
            string query,
            IDictionary<string, object> parameters)
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await reader.Map();
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }

        public async Task<IEnumerable<TPoco>> FindAsync<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await MetadataProvider.CreateMapperAsync<TPoco>()
                    .MapQueryAsync(reader);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }

        public async Task<TPoco> FirstOrDefaultAsync<TPoco>(
            string query,
            IDictionary<string, object> parameters)
            where TPoco : class
        {
            try
            {
                await OpenConnectionAsync();
                var command = (DbCommand) CreateTextCommand(query, parameters);
                var reader = await command.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await MetadataProvider.CreateMapperAsync<TPoco>()
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