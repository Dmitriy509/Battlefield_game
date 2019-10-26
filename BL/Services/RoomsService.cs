using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using BL.Interfaces;
using DL.Models;
using BL.Models;
using DL;
using static DL.Enums.StateEnums;
namespace BL.Services
{
    public class RoomsService:IRoomsService
    {
        DataManager _dm;
        public RoomsService()
        {
            _dm = new DataManager();
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
                        if (r.player1 != null) _dm.Ps.InitPlayer(r.player1);
                        if (r.player2 != null) _dm.Ps.InitPlayer(r.player2);
                        _dm.Rs.DeleteRoom(r);
                    }
                });

            }

        }

        public RoomsList GetInfoRooms(string playername)
        {

            _dm.Ps.GetPlayer(playername, true);
            updateRooms(playername);
            //form two lists room names and player count in rooms
            var list = _dm.Rs.GetAllRooms().Select(u => new
            {
                n = u.Name,
                pcount = (u.player2 == null ? 1 : 2)
            });
            RoomsList res = new RoomsList();
            res.RoomNames= list.Select(u => u.n).ToList();
            res.Player_Count= list.Select(u => u.pcount).ToList();       
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

            player.room = room;
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

            player.room = room;
            player.state = (sbyte)Player_States.editships;
            room.status = (sbyte)Game_States.editships;
            return new string[] { "FieldEditorView","" };
        }



    }
}
