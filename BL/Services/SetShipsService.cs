using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BL.Interfaces;
using BL.Models;
using DL.Enums;
using DL.Models;
using static DL.Enums.StateEnums;
namespace BL.Services
{
   public class SetShipsService: commonSrv, ISetShipsService
    {


       public string CheckGameState(string playername)
        {

            Player p= _dm.Ps.GetPlayer(playername);
            if (p == null) return "~/Login/Login";
            return LoginStateMachine(p);

        }



        public SetShipsService()
        {

        }

        public string[] UpdateRoom(string playername)
        {

            Player p1 = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(p1.roomid);
            Player p2 = _dm.Rs.GetPlayer2(p1,r);
            string p2res = "";


            Func<bool> checkDisconnect = () =>
              {

                  if (getInterval(p2.date, Parameters.PlayerDisconnect, '>'))
                  {
                      if (getInterval(p2.date, Parameters.WaitReconnect, '<'))
                      {
                          p2res = "Игрок отключился ожидаем подключения";
                          return true;
                      }
                      else
                      {
                          r.player1id = p1.id;
                          r.player2id = null;
                          r.status = (sbyte)Game_States.waitingplayer;
                          p2res = "Ждем нового игрока";
                          return true;
                      }

                  }
                  else return false;

              };


            Func<string> waitingplayer = () =>
            {
                p2res = "Ожидаем игрока";
                return "";
            };

            Func<string> editships = () =>
            {

                if (checkDisconnect()) return "";
                    
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

            };

            Func<string> readytoplay = () =>
            {
                    p2res = p2.login + " - Готов";
                    return "/Game/StartGame";

            };
            Func<string> readytoreplay = () =>
            {

                if (checkDisconnect()) return "";


                if (p1.state==(sbyte)Player_States.readytoreplay)
                 {
                    p1.state = (sbyte)Player_States.editships;
                 }
                



                if(p1.state == (sbyte)Player_States.editships&&p2.state == (sbyte)Player_States.editships)
                {
                    r.status = (sbyte)Game_States.editships;
                }
                p2res = "Ожидаем игрока";
                return "";
            };
            var gamestates = new Dictionary<sbyte, Func<string>>(4);
            gamestates.Add((sbyte)Game_States.waitingplayer, waitingplayer);
            gamestates.Add((sbyte)Game_States.editships, editships);
            gamestates.Add((sbyte)Game_States.readytoplay, readytoplay);
            gamestates.Add((sbyte)Game_States.readytoreplay, readytoreplay);
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
            Room room =_dm.Rs.GetRoom(player.roomid);
            
            Player player2 = _dm.Rs.GetPlayer2(player, room);
            if (player2 == null)
            {
                _dm.Rs.DeleteRoom(room);
                _dm.Ps.InitPlayer(player);

            }
            else
            {
                if (room.player1id == player.id)
                {

                    room.player1id = room.player2id;
                    room.player2id = null;
                }
                else room.player2id = null;
                _dm.Ps.InitPlayer(player);
                room.status = (sbyte)Game_States.waitingplayer;
                Task task = Task.Run(() =>
                {
                    Thread.Sleep(6000);
                    TimeSpan ts = DateTime.Now - room.updTime;
                    if (ts.TotalMilliseconds > 5000)
                    {
                        room.player1id = null;
                        room.player2id = null;
                        _dm.Ps.InitPlayer(player2);
                        _dm.Rs.DeleteRoom(room);
                    }
                });
            }


        }

    }
}
