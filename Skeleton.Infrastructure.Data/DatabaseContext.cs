using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Common;
using Skeleton.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Data
{
    [DebuggerDisplay("DatabaseName = {Configuration.Name}")]
    public abstract class DatabaseContext : DisposableBase
    {
        private readonly DataAdapter _adapter;
        private IDbCommand _command;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        protected DatabaseContext(
            ILogger logger,
            IDatabaseConfiguration configuration,
            IMetadataProvider metadataProvider)
        {
            Logger = logger;
            Configuration = configuration;
            MetadataProvider = metadataProvider;
            _adapter = new DataAdapter(configuration);
        }

        public IDatabaseConfiguration Configuration { get; }

        public IDatabaseTransaction Transaction => new DatabaseTransaction(this);

        internal ILogger Logger { get; }

        internal IMetadataProvider MetadataProvider { get; }

        internal void BeginTransaction(IsolationLevel? isolationLevel)
        {
            if (_transaction != null) return;

            OpenConnection();

            _transaction = isolationLevel.HasValue
                ? _connection.BeginTransaction(isolationLevel.Value)
                : _connection.BeginTransaction();
        }

        internal void CommitTransaction()
        {
            _transaction?.Commit();
        }

        internal void DisposeTransaction()
        {
            if (_transaction == null) return;

            _transaction.Dispose();
            _transaction = null;
        }

        private void CloseConnection()
        {
            if (_connection == null)
                return;

            if (_transaction != null)
                return;

            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        internal IDbCommand CreateStoredProcedureCommand(ISqlCommand procStockCommand)
        {
            return CreateCommand(CommandType.StoredProcedure, procStockCommand);
        }

        internal IDbCommand CreateTextCommand(ISqlCommand sqlCommand)
        {
            return CreateCommand(CommandType.Text, sqlCommand);
        }

        internal void OpenConnection()
        {
            try
            {
                if (_connection == null)
                    _connection = _adapter.CreateConnection();

                if (_connection.ConnectionString.IsNullOrEmpty())
                    _connection.ConnectionString = Configuration.ConnectionString;

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
            catch (Exception ex)
            {
                _connection?.Close();

                Logger.Error(ex.Message);
                throw new DataAccessException(ex.Message, ex.InnerException);
            }
        }

        internal async Task OpenConnectionAsync()
        {
            try
            {
                if (_connection == null)
                    _connection = _adapter.CreateConnection();

                if (_connection.State != ConnectionState.Open)
                    await ((DbConnection) _connection).OpenAsync()
                        .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _connection?.Close();

                Logger.Error(ex.Message);
                throw new DataAccessException(ex.Message, ex.InnerException);
            }
        }

        private void AttachParameters(IDictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                var parameter = _adapter.CreateParameter();

                parameter.DbType = DbTypeMapper.Map[param.Value.GetType()];
                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value;

                _command.Parameters.Add(parameter);
            }
        }

        private IDbCommand CreateCommand(
            CommandType commandType,
            ISqlCommand sqlCommand)
        {
            sqlCommand.SqlQuery.ThrowIfNullOrEmpty(() => sqlCommand.SqlQuery);

            _command = _adapter.CreateCommand();
            _command.Connection = _connection;
            _command.CommandText = sqlCommand.SqlQuery;
            _command.CommandType = commandType;
            _command.CommandTimeout =
                Configuration.CommandTimeout;

            if (_transaction != null)
                _command.Transaction = _transaction;

            if (sqlCommand.Parameters != null)
                AttachParameters(sqlCommand.Parameters);

            return _command;
        }

        protected override void DisposeManagedResources()
        {
            CloseConnection();

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}