using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Data
{
    public sealed class AsyncDatabase : DatabaseContext, IAsyncDatabase
    {
        public AsyncDatabase(
            ILogger logger,
            IDatabaseConfiguration configuration,
            IMetadataProvider metadataProvider)
            : base(logger, configuration, metadataProvider)
        {
        }

        public async Task<int> ExecuteAsync(ISqlCommand command)
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();

                var dbCommand = CreateTextCommand(command) as DbCommand;

                return await dbCommand.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            });
        }

        public async Task<object> ExecuteScalarAsync(ISqlCommand command)
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();

                var dbCommand = CreateTextCommand(command) as DbCommand;
                var result = await dbCommand
                    .ExecuteScalarAsync()
                    .ConfigureAwait(false);

                return result is DBNull
                    ? null
                    : result;
            });
        }

        public async Task<TValue> ExecuteScalarAsync<TValue>(ISqlCommand command)
        {
            var result = await ExecuteScalarAsync(command);

            return result == null
                ? default(TValue)
                : result.ChangeType<TValue>();
        }

        public async Task<int> ExecuteStoredProcedureAsync(ISqlCommand command)
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();
                var dbCommand = CreateStoredProcedureCommand(command) as DbCommand;

                return await dbCommand.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            });
        }

        public async Task<IEnumerable<dynamic>> FindAsync(ISqlCommand command)
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();

                var dbCommand = CreateTextCommand(command) as DbCommand;
                var reader = await dbCommand.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await reader.Map()
                    .ConfigureAwait(false);
            });
        }

        public async Task<IEnumerable<TPoco>> FindAsync<TPoco>(ISqlCommand command)
            where TPoco : class
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();

                var dbCommand = CreateTextCommand(command) as DbCommand;
                var reader = await dbCommand.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await MetadataProvider
                    .CreateMapperAsync<TPoco>(reader)
                    .MapQueryAsync()
                    .ConfigureAwait(false);
            });
        }

        public async Task<TPoco> FirstOrDefaultAsync<TPoco>(ISqlCommand command)
            where TPoco : class
        {
            return await RetryPolicy.ExecuteAsync(async () =>
            {
                await OpenConnectionAsync();

                var dbCommand = CreateTextCommand(command) as DbCommand;
                var reader = await dbCommand.ExecuteReaderAsync()
                    .ConfigureAwait(false);

                return await MetadataProvider
                    .CreateMapperAsync<TPoco>(reader)
                    .MapSingleAsync()
                    .ConfigureAwait(false);
            });
        }
    }
}