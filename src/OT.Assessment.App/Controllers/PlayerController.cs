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

       


    }
}
