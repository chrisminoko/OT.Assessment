﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OT.Assessment.Model.Dto;
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
                var result = await _playerService.PublishCasinoWagerAsync(request);
                if (result.IsSuccessful)
                {
                    return Ok(result);
                }
                return BadRequest(new BaseResponse { IsSuccessful = true,Message=result.Message });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = "Service temporarily unavailable" });
            }
           
        }

        [HttpGet("{playerId}/casino")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetPlayerWagers( [FromRoute] Guid playerId, [FromQuery] PaginationRequest request)
        {
            try
            {
                // Validate request
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { error = "Service temporarily unavailable" });
                }

                // Get wagers
                var result = await _playerService.GetPlayerCasinoWagersAsync( playerId,request.PageSize,  request.Page);

                if (!result.Data.Any())
                    return StatusCode(500, new BaseResponse { IsSuccessful = false, Message = "" });

                //if (!result.Data.Data.Any())
                //    return NotFound(new BaseResponse { IsSuccessful = false, Message = "" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { error = "Service temporarily unavailable" });
            }
        }


        [HttpGet("topSpenders")]
        [ProducesResponseType(typeof(IEnumerable<TopSpenderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopSpenders([FromQuery] int count = 10)
        {
            try
            {
               

                var result = await _playerService.GetTopSpendersAsync(count);
                if (!result.IsSuccess)
                {
                  
                    return StatusCode(500, new { success = false, message = result.Error});
                }

                return Ok(new   { success = true, data = result.Data });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new  { success = false, message = "An unexpected error occurred" });
            }
        }

        [HttpPost("AddPlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] PlayerCreateRequest request)
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

                return StatusCode(StatusCodes.Status503ServiceUnavailable,  new { error = "Service temporarily unavailable" });
            }

        }



    }
}
