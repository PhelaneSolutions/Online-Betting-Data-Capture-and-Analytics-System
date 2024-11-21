using Microsoft.AspNetCore.Mvc;
using OT.Assessment.App.Interfaces.Service;
using OT.Assessment.App.Models;
namespace OT.Assessment.App.Controllers
{
  
    [ApiController]
    [Route("api/player")]
    public class PlayerController : ControllerBase
    {
        private readonly ICasinoWagerService _casinoWagerService;
        public PlayerController(ICasinoWagerService casinoWagerService)
        {
            _casinoWagerService = casinoWagerService;
        }       

        [HttpPost("casinoWager")]
        public async Task<IActionResult> PostCasinoWager([FromBody] CasinoWager wager)
        {
            if (wager == null) return BadRequest("Wager is null");
            await _casinoWagerService.PublishWagerAsync(wager);
            return Ok();
        }
        [HttpGet("{playerId}/casino")]
        public async Task<IActionResult> GetPlayerCasinoWagers(Guid playerId, int pageSize, int page)
        {
            var result = await _casinoWagerService.GetPlayerWagerAsync(playerId,pageSize,page);
            return Ok(result);
        }
        [HttpGet("topSpenders")]
        public async Task<IActionResult> GetTopSpenders(int count = 10)
        {
            var result = await _casinoWagerService.GetTopSpendersAsync(count);
            return Ok(result);
        }
    }
}
