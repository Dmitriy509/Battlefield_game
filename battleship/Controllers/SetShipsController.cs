using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces;
using BL.Services;

namespace battleship.Controllers
{
    public class SetShipsController : Controller
    {
        ISetShipsService _srv;
        public SetShipsController()
        {

            _srv = new SetShipsService();
        }


        public IActionResult FieldEditorView()
        {

            return View();
        }



        public JsonResult UpdateInfoRoom(string playername)
        {
            if(playername==""||playername==null)
            {


            }

            string[] res = _srv.UpdateRoom(playername);
            //room.updTime = DateTime.Now;
            return Json(new { player2status = res[1], gamestatus = res[0] });
   
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