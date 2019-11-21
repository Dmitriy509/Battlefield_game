using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Models;
using DL;
using DL.Models;

using static DL.Enums.StateEnums;
namespace BL.Services
{
    public class RoomsService:commonSrv, IRoomsService
    {

        private readonly ILogger _logger;
        public RoomsService(ILogger logger)
        {
            _logger = logger;
        }

        public string CheckGameState(string playername)
        {

            Player p = _dm.Ps.GetPlayer(playername);
            if (p == null) return "~/Login/Login";
            return LoginStateMachine(p);

        }




        void updateRooms(string playername)
        {
            // if
            string first = _dm.Ps.GetAllPlayers().First(u => u.state == (sbyte)Player_States.signin).login;
            if (first == playername)
            {
                Task task = Task.Run(() =>
                {
                    var rooms = _dm.Rs.GetAllRooms().Where(u => (DateTime.Now - u.updTime).TotalSeconds > 40);
                    foreach (var r in rooms)
                    {
                        if (r.player1id != null) _dm.Ps.InitPlayer( _dm.Ps.GetPlayer(r.player1id));
                        if (r.player2id != null) _dm.Ps.InitPlayer(_dm.Ps.GetPlayer(r.player1id));
                        _dm.Rs.DeleteRoom(r);
                    }
                });

            }

        }

        public RoomsList GetInfoRooms(string playername)
        {

            _dm.Ps.GetPlayer(playername, true);
            updateRooms(playername);
            RoomsList res = new RoomsList();
            res.RoomNames = _dm.Rs.GetAllRooms().Where(u => u.status == (sbyte)Game_States.waitingplayer).Select(u => u.Name).ToList();
            res.Player_Count = _dm.Ps.GetAllPlayers().Count();
            res.Game_Count = _dm.Rs.GetAllRooms().Where(u => u.status == (sbyte)Game_States.playing).Count();
          
            return res;
        }


        public string [] CreateRoom(string roomName, string playername)
        {
            //string sss = getCookies();
            
            Room room = _dm.Rs.AddRoom(roomName);
            if (room == null)
            {
                // ViewBag.errmsg = "Комната с таким названием уже существует";
                return new string[] { "Rooms", "Комната с таким названием уже существует" };
            }

            Player player = _dm.Ps.GetPlayer(playername, true);
            if (!_dm.Rs.AddPlayer(player, room))
            {
                _dm.Rs.DeleteRoom(room);
              //  ViewBag.errmsg = "Возникла ошибка попробуйте еще раз";
                return new string[] { "Rooms", "Возникла ошибка попробуйте еще раз" };
            }

            player.roomid = room.id;
            player.state = (sbyte)Player_States.editships;
            room.status = (sbyte)Game_States.waitingplayer;

            return new string[] { "FieldEditorView", "" };
        }


        public string [] EnterTheRoom(string roomname, string playername)
        {
            Room room = _dm.Rs.GetRoom(roomname);
            if (room == null)
            {
               // ViewBag.errmsg = "Возникла ошибка попробуйте еще раз";
                return  new string[] { "Rooms", "Возникла ошибка войдите в игру снова" };
            }
            Player player = _dm.Ps.GetPlayer(playername, true);
            if (player == null)
            {
              //  ViewBag.errmsg = "Возникла ошибка войдите в игру снова";
                return new string[] { "~/Login/Login", "Возникла ошибка войдите в игру снова" };
            }

            if (!_dm.Rs.AddPlayer(player, room))
            {
                return new string[] { "Rooms", "Возникла ошибка попробуйте еще раз" };
            }

            player.roomid = room.id;
            player.state = (sbyte)Player_States.editships;
            room.status = (sbyte)Game_States.editships;
            return new string[] { "FieldEditorView","" };
        }



    }
}
