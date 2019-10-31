using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BL.Interfaces;
using BL.Models;
using DL;
using DL.Models;
using static DL.Enums.StateEnums;

namespace battleship.Controllers
{
    public class TestController : Controller
    {

        DataManager _dm;
        public TestController()
        {
            _dm = new DataManager();
        }

        public IActionResult test()
        {
            _dm.Ps.AddPlayer("Petya");
            _dm.Ps.AddPlayer("jora");
            Player p1 = _dm.Ps.GetPlayer("Petya", true);
            Player p2 = _dm.Ps.GetPlayer("jora", true);
            Room r = _dm.Rs.AddRoom("komnata");
            _dm.Rs.AddPlayer(p1, r);
            _dm.Rs.AddPlayer(p2, r);
            p1.roomid = r.id;
            p2.roomid = r.id;

            if (r != null)
            {
             
                if (p2 != null) _dm.Ps.InitPlayer(p2);
                _dm.Rs.DeleteRoom(r);
            }
            _dm.Ps.InitPlayer(p1);


            Player p11 = _dm.Ps.GetPlayer("Petya", true);

            if (p11.roomid == null)
            {
                _dm.Ps.InitPlayer(p11);
            }



            return Redirect("~/Rooms/Rooms");
        }



        public JsonResult Ready(string playername)
        {

            //Redirect(ResultWindow)
            Player player = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(player.roomid);
            // _gamesrv.Ps.AddPlayer("bbb");
            //  Player player2 = _gamesrv.Rs.GetPlayer2();
            //  player2.state = (sbyte)Player_States.editships;
            // r.player2 = player2;
            //  player2.room = r;
            // r.status = (sbyte)Game_States.editships;
            if (r.player1id == player.id)
            {
                int[] Xarr = { 0, 0, 0, 0, 0, 0, 0, 2, 3, 4, 2, 3, 2, 3, 2, 3, 6, 6, 6, 6 };
                int[] Yarr = { 0, 1, 2, 3, 5, 6, 7, 0, 0, 0, 2, 2, 4, 4, 6, 6, 0, 2, 4, 6 };
                for (int i = 0; i < Xarr.Length; i++)
                {
                    player.field[Yarr[i]][Xarr[i]] = 2;
                }
            }
            else
            {
                int[] Xarr1 = { 8, 8, 8, 8, 8, 8, 8, 2, 3, 4, 2, 3, 2, 3, 2, 3, 6, 6, 6, 6 };
                int[] Yarr1 = { 0, 1, 2, 3, 5, 6, 7, 0, 0, 0, 2, 2, 4, 4, 6, 6, 0, 2, 4, 6 };
                for (int i = 0; i < Xarr1.Length; i++)
                {
                    player.field[Yarr1[i]][Xarr1[i]] = 2;
                }
            }
            // r.status = (sbyte)Game_States.readytoplay;
            //player2.state = (sbyte)Player_States.readytoplay;
            player.state = (sbyte)Player_States.readytoplay;

            return Json(new { aaa = 1 });
        }
    }
}