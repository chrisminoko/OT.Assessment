using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OT.Assessment.Model.Request;
using OT.Assessment.Model.Response;
using OT.Assessment.Services.BusinessLogic.Interfaces;

namespace OT.Assessment.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        public ProviderController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        [HttpPost("provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreateCasionWager([FromBody] ProviderCreateRequest request)
        {
            try
            {
                var result = await _playerService.CreateProviderAsync(request);
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
