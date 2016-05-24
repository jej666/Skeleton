using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data.Configuration;

namespace Skeleton.Infrastructure.Data
{
    public abstract class DatabaseContext : DisposableBase
    {
        private readonly DataAdapter _adapter;
        private readonly IDatabaseConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ITypeAccessorCache _typeAccessorCache;
        private IDbCommand _command;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        protected DatabaseContext(
            IDatabaseConfiguration configuration,
            ITypeAccessorCache typeAccessorCache,
            ILogger logger)
        {
            _configuration = configuration;
            _typeAccessorCache = typeAccessorCache;
            _logger = logger;
            _adapter = new DataAdapter(configuration);
        }

        public IDatabaseConfiguration Configuration
        {
            get { return _configuration; }
        }

        public IDatabaseTransaction Transaction
        {
            get { return new DatabaseTransaction(this); }
        }

        internal ILogger Logger
        {
            get { return _logger; }
        }

        internal ITypeAccessorCache TypeAccessorCache
        {
            get { return _typeAccessorCache; }
        }

        internal void BeginTransaction(IsolationLevel? isolationLevel)
        {
            if (_transaction != null) return;

            OpenConnection();

            _transaction = isolationLevel.HasValue ? 
                _connection.BeginTransaction(isolationLevel.Value) : 
                _connection.BeginTransaction();
        }

        internal void CommitTransaction()
        {
            if (_transaction != null)
                _transaction.Commit();
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

        internal IDbCommand CreateStoredProcedureCommand(
            string procedureName,
            IDictionary<string, object> parameters)
        {
            return CreateCommand(CommandType.StoredProcedure, procedureName, parameters);
        }

        internal IDbCommand CreateTextCommand(
            string commandText,
            IDictionary<string, object> parameters)
        {
            return CreateCommand(CommandType.Text, commandText, parameters);
        }

        internal void OpenConnection()
        {
            try
            {
                if (_connection == null)
                    _connection = _adapter.CreateConnection();

                if (_connection.ConnectionString.IsNullOrEmpty())
                    _connection.ConnectionString = _configuration.ConnectionString;

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
            catch (Exception ex)
            {
                if (_connection != null)
                    _connection.Close();

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
                if (_connection != null)
                    _connection.Close();

                Logger.Error(ex.Message);
                throw new DataAccessException(ex.Message, ex.InnerException);
            }
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
            string commandText,
            IDictionary<string, object> parameters)
        {
            commandText.ThrowIfNullOrEmpty(() => commandText);

            _command = _adapter.CreateCommand();
            _command.Connection = _connection;
            _command.CommandText = commandText;
            _command.CommandType = commandType;
            _command.CommandTimeout =
                _configuration.CommandTimeout;

            if (_transaction != null)
                _command.Transaction = _transaction;

            if (parameters != null)
                AttachParameters(parameters);

            return _command;
        }
    }
}