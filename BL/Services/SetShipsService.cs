using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.Models;
using DL;
using DL.Models;
using static DL.Enums.StateEnums;
namespace BL.Services
{
   public class SetShipsService:ISetShipsService
    {
        DataManager _dm;
        public SetShipsService()
        {
            _dm = new DataManager();
        }

        public string[] UpdateRoom(string playername)
        {

            Player p1 = _dm.Ps.GetPlayer(playername, true);
            Room r = p1.room;
            Player p2 = _dm.Rs.GetPlayer2(p1);
            string p2res = "";
            var gamestates = new Dictionary<sbyte, Func<string>>(3);
            //gamestates.Add((sbyte)Game_States.waitingplayer)
            Func<string> waitingplayer = () =>
            {
                p2res = "Ожидаем игрока";
                return "";
            };

            Func<string> editships = () =>
            {
                try
                {
                    TimeSpan ts = DateTime.Now - p2.date;
                    if (ts.TotalSeconds > 10)
                    {
                        if (ts.TotalSeconds < 120)
                        {
                            p2res = "Игрок отключился ожидаем подключения";
                            return "";
                        }
                        else
                        {
                            r.player1 = p1;
                            r.player2 = null;
                            r.status = (sbyte)Game_States.waitingplayer;
                            p2res = "Ждем нового игрока";
                            return "";
                        }

                    }

                    if (p2.state == (sbyte)Player_States.readytoplay)
                    {
                        if (p1.state == (sbyte)Player_States.readytoplay)
                        {
                            r.status = (sbyte)Game_States.readytoplay;
                            p2res = p2.login + " - Готов";
                            return "";
                        }
                        else
                        {
                            p2res = p2.login + " - Готов";
                            return "";
                        }
                    }
                    else
                    {
                        p2res = p2.login;
                        return "";
                    }
                }
                catch
                {
                    r.player1 = p1;
                    r.player2 = null;
                    r.status = (sbyte)Game_States.waitingplayer;
                    p2res = "Игрок вышел, ждем нового игрока";
                    return "";
                }
            };

            Func<string> readytoplay = () =>
            {
                try
                {
                    p2res = p2.login + " - Готов";
                    return "/Game/StartGame";
                }
                catch
                {
                    r.player1 = p1;
                    r.player2 = null;
                    r.status = (sbyte)Game_States.waitingplayer;
                    p2res = "Игрок вышел, ждем нового игрока";
                    return "";
                }
            };

            gamestates.Add((sbyte)Game_States.waitingplayer, waitingplayer);
            gamestates.Add((sbyte)Game_States.editships, editships);
            gamestates.Add((sbyte)Game_States.readytoplay, readytoplay);
            r.updTime = DateTime.Now;
            return new string[] { gamestates[r.status](), p2res };

        }


        public bool GetCoords(string playername, int[] Xarr, int[] Yarr)
         {
            Player player = _dm.Ps.GetPlayer(playername, true);
            if (player == null)
            {
                //ViewBag.errmsg = "Возникла ошибка войдите в игру снова";
                //   return "Login";
                return false;
            }
            for (int i = 0; i < Xarr.Length; i++)
            {
                player.field[Yarr[i]][Xarr[i]] = (sbyte)Field_Cell_States.ship;
            }
            player.state = (sbyte)Player_States.readytoplay;

            return true;

        }

        public void LeaveRoom(string playername)
        {

            Player player = _dm.Ps.GetPlayer(playername, true);
            Room room = player.room;
            Player player2 = _dm.Rs.GetPlayer2(player);
            if (player2 == null)
            {
                _dm.Rs.DeleteRoom(room);
                _dm.Ps.InitPlayer(player);

            }
            else
            {
                if (room.player1 == player)
                {
                    room.player1 = null;
                    room.player1 = room.player2;
                    room.player2 = null;
                }
                else room.player2 = null;
                _dm.Ps.InitPlayer(player);
                Task task = Task.Run(() =>
                {
                    Thread.Sleep(6000);
                    TimeSpan ts = DateTime.Now - room.updTime;
                    if (ts.TotalMilliseconds > 5000)
                    {
                        room.player1 = null;
                        room.player2 = null;
                        _dm.Ps.InitPlayer(player2);
                        _dm.Rs.DeleteRoom(room);
                    }
                });
            }


        }

    }
}
