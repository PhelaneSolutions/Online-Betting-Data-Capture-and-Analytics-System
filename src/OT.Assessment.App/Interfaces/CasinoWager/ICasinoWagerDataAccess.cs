using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Interfaces
{
    public interface ICasinoWagerDataAccess
    {
        Task InsertCasinoWagerAsync(CasinoWager casinoWager);
        Task<PaginatedResponse<CasinoWagerResponse>> GetPlayerWagerAsync(Guid playerId, int PageSize, int page);
        Task<IEnumerable<Player>> GetTopSpendersAsync (int count);
    }
}