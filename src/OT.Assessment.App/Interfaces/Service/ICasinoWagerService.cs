using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Interfaces.Service
{
    public interface ICasinoWagerService
    {
        Task PublishWagerAsync(CasinoWager wager);
        Task<PaginatedResponse<CasinoWagerResponse>> GetPlayerWagerAsync(Guid player,int pageSize, int page);
        Task<IEnumerable<Player>> GetTopSpendersAsync(int count);
    }
}