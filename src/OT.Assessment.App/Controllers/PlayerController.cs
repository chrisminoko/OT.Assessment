using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OT.Assessment.Model.Entities;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
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


        [HttpPost("casinowager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateCasionWager([FromBody] CasinoWagerRequest request)
        {
            try
            {
                var result = await _playerService.CreateCasinoWagerAsync(request);
                if (result.IsSuccessful)
                {
                    return Ok(result);
                }
                return BadRequest(new BaseResponse { IsSuccessful = true,Message=result.Message });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { error = "Service temporarily unavailable" });
            }
           
        }

        [HttpPost("player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCreateRequest request)
        {
            try
            {
                var result = await _playerService.CreatePlayerAsync(request);
                if (result.IsSuccessful)
                {
                    return Ok(result);
                }
                return BadRequest(new BaseResponse { IsSuccessful = true, Message = result.Message });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { error = "Service temporarily unavailable" });
            }

        }

    }
}
