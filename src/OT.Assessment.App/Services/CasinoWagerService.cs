using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OT.Assessment.App.Interfaces;
using OT.Assessment.App.Interfaces.DatabaseOperations;
using OT.Assessment.App.Interfaces.RabbitMQ;
using OT.Assessment.App.Interfaces.Service;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Services
{
    public class CasinoWagerService : ICasinoWagerService 
    {
        private readonly IPublishWagerAsync _publishWagerAsync;
        private readonly ICasinoWagerDataAccess _casinowagerDataAccess;

        public CasinoWagerService(IPublishWagerAsync publishWagerAsync, ICasinoWagerDataAccess casinoWagerDataAccess)
        {
            _publishWagerAsync = publishWagerAsync;
            _casinowagerDataAccess = casinoWagerDataAccess; 
        }

        public async Task PublishWagerAsync(CasinoWager wager)
        {
            await _publishWagerAsync.PublishWagerAsync(wager);
            await _casinowagerDataAccess.InsertCasinoWagerAsync(wager);
        }

        public async Task<PaginatedResponse<CasinoWagerResponse>> GetPlayerWagerAsync(Guid playerId, int pageSize, int page)
        {
            return await _casinowagerDataAccess.GetPlayerWagerAsync(playerId,pageSize,page);
        }
        
        public async Task<IEnumerable<Player>> GetTopSpendersAsync(int count)
        {
            return await _casinowagerDataAccess.GetTopSpendersAsync(count);
        }
    }
}