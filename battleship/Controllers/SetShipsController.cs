using System;
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
            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);

            string res = _srv.CheckGameState(player_id);
            if (res == "~/SetShips/FieldEditorView")
            {
                List<SendShips> ships = _srv.CheckPlayerReady(player_id);
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


        [HttpPost]
        public JsonResult UpdateInfoRoom(string player_id)
        {
            if(player_id == ""|| player_id == null)
            {
                _logger.LogError("SetShips/UpdateInfoRoom player_id is null");
            }

            var res = _srv.UpdateRoom(player_id); 
            if(res==null) return Json(new { player2name = "", player2status = "", gamestatus = "" });
            return Json(new { player2name = res["player2name"], player2status = res["player2status"], gamestatus = res["gamestatus"] });
   
        }


        public JsonResult GetShipsCoords(string player_id, int[] Xarr, int[] Yarr)
        {

            if (player_id == null || player_id == "")
                return Json(new { status = false });
            if(Xarr.Length<=0|| Yarr.Length <= 0) return Json(new { status = false });


            bool res = _srv.GetCoords(player_id, Xarr, Yarr);

            return Json(new { status = res });

        }


        [HttpPost]
        public IActionResult LeaveRoom(string player_id)
        {
            if (player_id == "" || player_id == null)
            {

                Redirect("~/Login/Login");
            }
            _srv.LeaveRoom(player_id);

  
            return Redirect("~/Rooms/Rooms");
        }

    }
}