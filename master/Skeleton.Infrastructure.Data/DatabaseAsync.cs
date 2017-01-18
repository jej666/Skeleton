using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;
using Skeleton.Abstraction.Reflection;

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

        public async Task<int> ExecuteAsync(ISqlCommand command)
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateTextCommand(command);

                return await dbCommand.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }


        public async Task<object> ExecuteScalarAsync(ISqlCommand command)
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateTextCommand(command);
                var result = await dbCommand.ExecuteScalarAsync()
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

        public async Task<TValue> ExecuteScalarAsync<TValue>(ISqlCommand command)
        {
            var result = await ExecuteScalarAsync(command);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
        }

        public async Task<int> ExecuteStoredProcedureAsync(ISqlCommand procStockCommand)
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateStoredProcedureCommand(
                    procStockCommand);

                return await dbCommand.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message);
                throw new DataAccessException(e);
            }
        }

        public async Task<IEnumerable<dynamic>> FindAsync(ISqlCommand command)
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateTextCommand(command);
                var reader = await dbCommand.ExecuteReaderAsync()
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
            ISqlCommand command)
            where TPoco : class
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateTextCommand(command);
                var reader = await dbCommand.ExecuteReaderAsync()
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
            ISqlCommand command)
            where TPoco : class
        {
            try
            {
                await OpenConnectionAsync();
                var dbCommand = (DbCommand) CreateTextCommand(command);
                var reader = await dbCommand.ExecuteReaderAsync()
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