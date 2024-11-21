using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using OT.Assessment.App.Interfaces.DatabaseOperations;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Repository
{

    public class DatabaseOperationsRepository : IDatabaseOperations
    {
        private readonly string _connectionString;
        private readonly IMemoryCache _cache;

        public DatabaseOperationsRepository(IConfiguration configuration, IMemoryCache cache)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
            _cache = cache;
        }
        public async Task ExecuteNonQuerryAsync(string storedProcedure, Action<SqlCommand> configureCommand)
        {
            await using var connection = await CreateAndOpenConnectionAsync();
            await using var command = CreateCommand(connection, storedProcedure, configureCommand);
            await command.ExecuteNonQueryAsync();
        }
        public async Task<T> ExecuteReaderAsync<T>(string storedProcedure, Action<SqlCommand> configureCommand, Func<SqlDataReader, T> readData)
        {
             await using var connection = await CreateAndOpenConnectionAsync();
             await using var command = CreateCommand(connection,storedProcedure,configureCommand);
             await using var reader = await command.ExecuteReaderAsync();
             return readData(reader);   
        }
        
        public async Task<T> ExecutereaderWithCacheAsync<T>(string cachKey, string storedProcedure, Action<SqlCommand> configureCommand, Func<SqlDataReader, T> readData, TimeSpan cacheDuration)
        {
            if(!_cache.TryGetValue(cachKey, out T cachedData))
            {
                cachedData = await ExecuteReaderAsync(storedProcedure,configureCommand,readData);
                _cache.Set(cachKey, cachedData, cacheDuration);
            }
            return cachedData;
        }
        private SqlCommand CreateCommand(SqlConnection connection, string storedProcedure, Action<SqlCommand> configureCommand)
        {
            var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            configureCommand(command);
            return command;
        }
        private async Task<SqlConnection> CreateAndOpenConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public void AddCasinoWagerParameter(SqlCommand command, CasinoWager wager)
        {
            command.Parameters.AddWithValue("@WagerId", wager.WagerId);
            command.Parameters.AddWithValue("@AccountId", wager.AccountId);
            command.Parameters.AddWithValue("@Theme", wager.Theme);
            command.Parameters.AddWithValue("@Provider", wager.Provider);
            command.Parameters.AddWithValue("@GameName", wager.GameName);
            command.Parameters.AddWithValue("@TransactionId", wager.TransactionId);
            command.Parameters.AddWithValue("@BrandId", wager.BrandId);
            command.Parameters.AddWithValue("@ExternalReferenceId", wager.ExternalReferenceId);
            command.Parameters.AddWithValue("@TransactionTypeId", wager.TransactionTypeId);
            command.Parameters.AddWithValue("@Amount", wager.Amount);
            command.Parameters.AddWithValue("@CreatedDateTime", wager.CreatedDateTime);
            command.Parameters.AddWithValue("@NumberOfBets", wager.NumberOfBets);
            command.Parameters.AddWithValue("@SessionData", wager.SessionData);
            command.Parameters.AddWithValue("@Duration", wager.Duration);
            command.Parameters.AddWithValue("@CountryCode",wager.CountryCode);
            command.Parameters.AddWithValue("@Username",wager.Username);
        }
        public void AddDictionaryParameters(SqlCommand command, IDictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
            }
        }
    }
}