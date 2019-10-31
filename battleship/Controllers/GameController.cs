using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public GameController()
        {
   
           // object g = new { aaa = "dsf" };
             _gs = new GameService();
        }
    
        public IActionResult GameView(string player1name)
        {

            if(player1name==null)
            { 
               player1name = CookiesGetSet.getCookies(HttpContext);
            }



            string checkres = _gs.CheckGameState(player1name);
            if (checkres != "~/Game/GameView")
                 return Redirect(checkres);




            StartGameData res= _gs.InitGame(player1name);
               
                ViewBag.player1name = player1name;
                ViewBag.player2name = res.player2name;
                ViewBag.p1field = res.player1field;
                ViewBag.p2field = res.player2field;
                ViewBag.waitreplay = Parameters.WaitReplayGame;
                ViewBag.movetime = Parameters.MoveTime;
                return View();
        }

        public ActionResult StartGame()
        {
            string playername = CookiesGetSet.getCookies(HttpContext);
            // string playername=Loginc
            string p2 = _gs.StartGame(playername);
          //  ViewBag.player1name = playername;
           // ViewBag.player2name = p2;
            return Redirect("GameView?player1name="+ playername);
        }





        [HttpPost]
        public JsonResult Fire(string playername, int x, int y)
        {

            FireResults res = _gs.Fire(playername, x, y);

            return Json(new { cells = res.XCoords, rows = res.YCoords, fireresult = res.FireRes, shipcount = res.p2shipscount });

        }

        [HttpPost]
        public JsonResult UpdateGameProcess(string playername, sbyte curmovestate)
        {
 
            GameProcessData res = _gs.GameProcessStateMachine(playername, curmovestate);
            if(res.gamestatus== "results")
            {
                return Json(new {gamestatus = res.gamestatus, gameresult = res.curmovestate==(sbyte)Moves_States.winner?"Победа":"Поражение"});
            }
            //_gamesrv.Sts.GameStateMachine(playername, ref curmovestate, _gamesrv, ref p2res, ref sendfield, ref shipsc);
            return Json(new { player2status = res.player2status, gamestatus = res.gamestatus, movestate = res.curmovestate, field = res.PlayerField, shipcount = res.shipscount });
        }


        [HttpPost]
        public JsonResult GiveUp(string playername)
        {

            _gs.GiveUp(playername);

         
            return Json(new {});
        }

    }
}