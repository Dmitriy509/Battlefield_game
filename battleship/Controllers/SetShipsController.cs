﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Services;
using BL.Models;
namespace battleship.Controllers
{
    public class SetShipsController : Controller
    {
        ISetShipsService _srv;
        private readonly ILogger _logger;
        public SetShipsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MyApp");
            _srv = new SetShipsService(_logger);
        }


        public IActionResult FieldEditorView()
        {
            string playername = CookiesGetSet.getCookies(HttpContext);

            string res = _srv.CheckGameState(playername);
            if (res == "~/SetShips/FieldEditorView")
            {
                List<SendShips> ships = _srv.CheckPlayerReady(playername);
                if (ships != null)
                {
                    ViewBag.Ready = true;
                    ViewBag.Ships = ships;
                }
                else
                {
                    ViewBag.Ready = false;
                    ViewBag.Ships = -1;
                }
                //   ViewBag.PlayerName = playername;
                return View();
            }
            else return Redirect(res);
        }



        public JsonResult UpdateInfoRoom(string playername)
        {
            if(playername==""||playername==null)
            {


            }

            var res = _srv.UpdateRoom(playername); 
            //room.updTime = DateTime.Now;
            return Json(new { player2name = res["player2name"], player2status = res["player2status"], gamestatus = res["gamestatus"] });
   
        }


        public JsonResult GetShipsCoords(string playername, int[] Xarr, int[] Yarr)
        {

            if (playername == null || playername == "") return Json(new { status = false });
            if(Xarr.Length<=0|| Yarr.Length <= 0) return Json(new { status = false });


            bool res = _srv.GetCoords(playername, Xarr, Yarr);

            return Json(new { status = res });

        }


        [HttpPost]
        public IActionResult LeaveRoom(string playername)
        {
            if (playername == "" || playername == null)
            {

            }
            _srv.LeaveRoom(playername);
            return Redirect("~/Rooms/Rooms");
        }

    }
}