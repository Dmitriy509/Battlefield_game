using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Services;
using BL.Models;

namespace battleship.Controllers
{
    public class RoomsController : Controller
    {
        
        IRoomsService _rs;
        private readonly ILogger _logger;
        public RoomsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MyApp");
            _rs = new RoomsService(_logger);
        }

        public IActionResult Rooms()
        {

            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);

            string res = _rs.CheckGameState(player_id);
            if (res == "~/Rooms/Rooms")
                return View();
            else return Redirect(res);
        }
  
        [HttpPost]
        public JsonResult GetInfoRooms(string player_id)
        {
            //string name = getCookies();
            if(player_id == ""|| player_id == null)
            {
                return null;
            }
            RoomsList res = _rs.GetInfoRooms(player_id);
           // return Json(_rs.GetInfoRooms(playername));
             return Json(new { roomnames = res.RoomNames, player_count = res.Player_Count, game_count=res.Game_Count });

        }

        [HttpPost]
        public IActionResult AddRoom(string roomName, string player_id)
        {
            if(roomName==""||roomName==null)
            {
              // ViewBag.errmsg = "";
                return View("Rooms");
            }
            if (player_id == "" || player_id == null)
            {
                // ViewBag.errmsg = "";
                _logger.LogError("Rooms/addRoom id игрока отсутствует");
                return Redirect("Login");
            }
            string[] res = _rs.CreateRoom(roomName, player_id);
            if(res[0]=="Rooms")
            {
                ViewBag.errmsg = res[1];
                return View("Rooms");
            }

            return Redirect("~/SetShips/FieldEditorView");

        }

        [HttpPost]
        public IActionResult EnterTheRoom(string roomname, string player_id)
        {
            if (roomname == "" || roomname == null)
            {
                // ViewBag.errmsg = "";
                return View("Rooms");
            }
            if (player_id == "" || player_id == null)
            {
                // ViewBag.errmsg = "";
                _logger.LogError("Rooms/EnterTheRoom id игрока отсутствует");
                return Redirect("Login");
            }
            string[] res = _rs.EnterTheRoom(roomname, player_id);
            if (res[0] == "Rooms")
            {
                ViewBag.errmsg = res[1];
                return View("Rooms");
            }
            if(res[0].Contains("Login"))
            {

                ViewBag.errmsg = res[1];
                return Redirect(res[0]);

            }
            return Redirect("~/SetShips/FieldEditorView");  
            
        }

    }
}