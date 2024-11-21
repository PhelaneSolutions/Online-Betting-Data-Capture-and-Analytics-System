using Microsoft.Data.SqlClient;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Interfaces.DatabaseOperations
{
    public interface IDatabaseOperations
    {
        Task ExecuteNonQuerryAsync(string storedProcedure, Action<SqlCommand> configureCommand);
        Task<T> ExecuteReaderAsync<T>(string storedProcedure,Action<SqlCommand> configureCommands, Func<SqlDataReader, T> readData);
        void AddCasinoWagerParameter(SqlCommand command, CasinoWager wager);
        void AddDictionaryParameters(SqlCommand command, IDictionary<string, object> parameters);
        Task<T> ExecutereaderWithCacheAsync<T>(string cachKey, string storedProcedure, Action<SqlCommand> configureCommands, Func<SqlDataReader, T> readData, TimeSpan cacheDuration);
    }
}