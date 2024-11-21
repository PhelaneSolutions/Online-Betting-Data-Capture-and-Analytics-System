using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OT.Assessment.App.Interfaces;
using OT.Assessment.App.Interfaces.DatabaseOperations;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.DataAccess
{
    public class CasinoWagerDataAccess :ICasinoWagerDataAccess
    {
        private readonly IDatabaseOperations _databaseOperations;
        private readonly ILogger _logger;
        public CasinoWagerDataAccess(IDatabaseOperations databaseOperations, ILogger<CasinoWagerDataAccess> logger)
        {
            _databaseOperations = databaseOperations;
            _logger = logger;
        }
        public async Task InsertCasinoWagerAsync(CasinoWager wager)
        {
            _logger.LogInformation("inserting casino wager with ID {WagerId}", wager.WagerId);
            await _databaseOperations.ExecuteNonQuerryAsync("InsertCasinoWager", command => 
            {
                _databaseOperations.AddCasinoWagerParameter(command, wager);
            });
        }
        public async Task<PaginatedResponse<CasinoWagerResponse>> GetPlayerWagerAsync(Guid playerId, int pageSize, int page)
        {
            var cacheKey = $"PlayerWagers_{playerId}_{pageSize}_{page}";
            return await _databaseOperations.ExecutereaderWithCacheAsync(cacheKey, "GetPlayerCasinoWagers" , command => 
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@PlayerId", playerId},
                    {"@PageSize", pageSize},
                    {"@Page", page}
                };
                _databaseOperations.AddDictionaryParameters(command, parameters);
            }, reader => 
            {
                var wagers = new List<CasinoWagerResponse>();
                int total = 0;

                while(reader.Read())
                {
                    wagers.Add(new CasinoWagerResponse
                    {
                        WagerId = reader.GetGuid(0),
                        Game = reader.GetString(1),
                        Provider = reader.GetString(2),
                        Amount = reader.GetDecimal(3),
                        CreatedDate = reader.GetDateTime(4)

                    });
                }
                    if (reader.NextResult() && reader.Read())
                     { 
                        total = reader.GetInt32(0); 
                     }

                    return new PaginatedResponse<CasinoWagerResponse>
                    {
                        Data = wagers,
                        page = page,
                        PageSize = pageSize,
                        Total = total,
                        TotalPages = (int)Math.Ceiling(total / (double)pageSize)
                    };
            }, TimeSpan.FromMinutes(5));
        }
        public async Task<IEnumerable<Player>> GetTopSpendersAsync(int count)
        {
            var cachKey = $"TopSpenders_{count}";
            return await _databaseOperations.ExecutereaderWithCacheAsync(cachKey, "GetTopSpenders", command =>
            {
                var parameter = new Dictionary<string , object>
                {
                    {"@Count", count}
                };
                _databaseOperations.AddDictionaryParameters(command, parameter);
            }, reader => 
            {
                var topSpender = new List<Player>();
                while (reader.Read())
                {
                    topSpender.Add(new Player
                    {
                        AccountId = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0),
                        Username = reader.IsDBNull(1) ? null : reader.GetString(1),
                        TotalAmountSpend = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                    });
                }
                return topSpender;
            }, TimeSpan.FromMinutes(5));
        }
    }
        
}
