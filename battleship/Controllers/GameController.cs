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
            string player1name = CookiesGetSet.getCookies(HttpContext);

            string checkres = _gs.CheckGameState(player1name);
            if (checkres != "~/Game/GameView")
                 return Redirect(checkres);




            StartGameData res= _gs.InitGame(player1name);
               
                ViewBag.player1name = player1name;
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

            string playername = CookiesGetSet.getCookies(HttpContext);
            // string playername=Loginc
            if (!_gs.StartGame(playername)) return Redirect(_gs.CheckGameState(playername));
          //  ViewBag.player1name = playername;
           // ViewBag.player2name = p2;
            return Redirect("GameView");
        }





        [HttpPost]
        public JsonResult Fire(string playername, int x, int y)
        {

            FireResults res = _gs.Fire(playername, x, y);

            return Json(new { cells = res.XCoords, rows = res.YCoords, movetime=res.movetime, fireresult = res.FireRes, shipcount = res.p2shipscount });

        }

        [HttpPost]
        public JsonResult UpdateGameProcess(string playername, sbyte curmovestate)
        {
 
            GameProcessData res = _gs.GameProcessStateMachine(playername, curmovestate);
            if(res.gamestatus== "results")
            {
                return Json(new {gamestatus = res.gamestatus, gameresult = res.curmovestate==(sbyte)Moves_States.winner?"win":"def"});
            }
            //_gamesrv.Sts.GameStateMachine(playername, ref curmovestate, _gamesrv, ref p2res, ref sendfield, ref shipsc);
            return Json(new { player2status = res.player2status, gamestatus = res.gamestatus, movestate = res.curmovestate, movetime = res.movetime, field = res.PlayerField, shipcount = res.shipscount });
        }


        [HttpPost]
        public JsonResult GiveUp(string playername)
        {

            _gs.GiveUp(playername);

         
            return Json(new {});
        }

    }
}