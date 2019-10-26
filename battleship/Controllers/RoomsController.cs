using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces;
using BL.Services;
using BL.Models;
namespace battleship.Controllers
{
    public class RoomsController : Controller
    {
        
        IRoomsService _rs;
        public RoomsController()
        {
            _rs = new RoomsService();
        }

        public IActionResult Rooms()
        {
            return View("Rooms");
        }
  
        [HttpPost]
        public JsonResult GetInfoRooms(string playername)
        {
            //string name = getCookies();
            if(playername==""||playername==null)
            {
                return null;
            }
            RoomsList res = _rs.GetInfoRooms(playername);
           // return Json(_rs.GetInfoRooms(playername));
             return Json(new { roomnames = res.RoomNames, player_count = res.Player_Count });

        }

        [HttpPost]
        public IActionResult AddRoom(string roomName, string playername)
        {
            if(roomName==""||roomName==null)
            {
              // ViewBag.errmsg = "";
                return View("Rooms");
            }
            if (playername == "" || playername == null)
            {
                // ViewBag.errmsg = "";
                //return Redirect("Login");
            }
            string[] res = _rs.CreateRoom(roomName, playername);
            if(res[0]=="Rooms")
            {
                ViewBag.errmsg = res[1];
                return View("Rooms");
            }

            return Redirect("~/SetShips/FieldEditorView");

        }

        [HttpPost]
        public IActionResult EnterTheRoom(string roomname, string playername)
        {
            if (roomname == "" || roomname == null)
            {
                // ViewBag.errmsg = "";
                return View("Rooms");
            }
            if (playername == "" || playername == null)
            {
                // ViewBag.errmsg = "";
                //return Redirect("Login");
            }
            string[] res = _rs.EnterTheRoom(roomname, playername);
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