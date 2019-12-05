using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Services;
using BL.Models;
using static DL.Enums.StateEnums;

namespace battleship.Controllers
{
    public class GameResultsController : Controller
    {
        IGameResultsService _grs;
        private readonly ILogger _logger;
        public GameResultsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MyApp");
            // object g = new { aaa = "dsf" };
            _grs = new GameResultsService(_logger);
        }


        [HttpPost]
        public IActionResult ExitGame(string player_id)
        {
            _grs.exitGame(player_id);
            return Redirect("~/Rooms/Rooms");
        }

        [HttpPost]
        public JsonResult ReplayGame(string player_id)
        {
            _grs.replayGame(player_id);
            return Json(new { });
        }

        [HttpPost]
        public JsonResult UpdateGameResultView(string player_id, bool fltimeisup)
        {
            string res = _grs.updateGameResult(player_id, fltimeisup);

            return Json(new {player2status = res });
        }


        [HttpPost]
        public JsonResult UpdatePlayer(string player_id)
        {
            _grs.updatePlayer(player_id);
            return Json(new { });
        }

    }
}