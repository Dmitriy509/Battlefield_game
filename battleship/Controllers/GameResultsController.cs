using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces;
using BL.Services;
using BL.Models;
using static DL.Enums.StateEnums;

namespace battleship.Controllers
{
    public class GameResultsController : Controller
    {
        IGameResultsService _grs;
        public GameResultsController()
        {

            // object g = new { aaa = "dsf" };
            _grs = new GameResultsService();
        }


        [HttpPost]
        public IActionResult ExitGame(string playername)
        {
            _grs.exitGame(playername);
            return Redirect("~/Rooms/Rooms");
        }

        [HttpPost]
        public JsonResult ReplayGame(string playername)
        {
            _grs.replayGame(playername);
            return Json(new { });
        }

        [HttpPost]
        public JsonResult UpdateGameResultView(string playername, bool fltimeisup)
        {
            string res = _grs.updateGameResult(playername, fltimeisup);

            return Json(new {player2status = res });
        }


        [HttpPost]
        public JsonResult UpdatePlayer(string playername)
        {
            _grs.updatePlayer(playername);
            return Json(new { });
        }

    }
}