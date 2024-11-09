using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Model.Entities;
using OT.Assessment.Services.BusinessLogic.Interfaces;
namespace OT.Assessment.App.Controllers
{
  
    [ApiController]
    public class PlayerController : ControllerBase
    {
        //POST api/player/casinowager

        //GET api/player/{playerId}/wagers

        //GET api/player/topSpenders?count=10
        //
        private readonly IPlayerService _playerService;
        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        [Route("AddPlayer")]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest("Player data is required.");
            }

            try
            {
                await _playerService.CreatePlayer(player);

                return CreatedAtAction(nameof(CreatePlayerAsync), new { id = player.Id }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the player.");
            }
        }

        [HttpGet]
        [Route("GetPlayerbyId")]
        public async Task<IActionResult> GetPlayerbyId(Guid Id) 
        {
         var response = await _playerService.GetPlayerById(Id);
        if (response == null)
            {
                return BadRequest();
            }
        
        return Ok(response);
        }


    }
}
