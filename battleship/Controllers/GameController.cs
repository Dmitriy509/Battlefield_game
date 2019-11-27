using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Services;
using BL.Models;
using DL.Enums;
using static DL.Enums.StateEnums;


namespace battleship.Controllers
{
    public class GameController : Controller
    {
        IGameService _gs;
        private readonly ILogger _logger;
        public GameController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MyApp");
            // object g = new { aaa = "dsf" };
            _gs = new GameService(_logger);
        }
    
        public IActionResult GameView()
        {
            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);

            string checkres = _gs.CheckGameState(player_id);
            if (checkres != "~/Game/GameView")
                 return Redirect(checkres);




            StartGameData res= _gs.InitGame(player_id);
               
                ViewBag.player1name = res.player1name;
                ViewBag.player2name = res.player2name;
                ViewBag.p1field = res.player1field;
                ViewBag.p2field = res.player2field;
              //  ViewBag.waitreplay = Parameters.WaitReplayGame;
                int min = Parameters.WaitReplayGame / 60;
                int sec = Parameters.WaitReplayGame % 60;
                ViewBag.waitreplay = min + ":" + (sec < 10 ? "0" + sec.ToString() : sec.ToString());



                min = Parameters.MoveTime / 60;
                sec = Parameters.MoveTime % 60;
                ViewBag.movetime = min+":" + (sec < 10 ? "0" + sec.ToString() : sec.ToString());
                return View();
        }


        
        public ActionResult StartGame()
        {

            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);
            // string playername=Loginc
            if (!_gs.StartGame(player_id)) return Redirect(_gs.CheckGameState(player_id));
          //  ViewBag.player1name = playername;
           // ViewBag.player2name = p2;
            return Redirect("GameView");
        }





        [HttpPost]
        public JsonResult Fire(string player_id, int x, int y)
        {

            FireResults res = _gs.Fire(player_id, x, y);

            return Json(new { cells = res.XCoords, rows = res.YCoords, movetime=res.movetime, fireresult = res.FireRes, shipcount = res.p2shipscount });

        }

        [HttpPost]
        public JsonResult UpdateGameProcess(string player_id, sbyte curmovestate)
        {
 
            GameProcessData res = _gs.GameProcessStateMachine(player_id, curmovestate);
            if(res.gamestatus== "results")
            {
                return Json(new {gamestatus = res.gamestatus, gameresult = res.curmovestate==(sbyte)Moves_States.winner?"win":"def"});
            }
            //_gamesrv.Sts.GameStateMachine(playername, ref curmovestate, _gamesrv, ref p2res, ref sendfield, ref shipsc);
            return Json(new { player2status = res.player2status, gamestatus = res.gamestatus, movestate = res.curmovestate, movetime = res.movetime, field = res.PlayerField, shipcount = res.shipscount });
        }


        [HttpPost]
        public JsonResult GiveUp(string player_id)
        {

            _gs.GiveUp(player_id);

         
            return Json(new {});
        }

    }
}