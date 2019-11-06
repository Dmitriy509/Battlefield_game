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

        public List <SendShips> CheckPlayerReady(string playername)
        {

            Player p = _dm.Ps.GetPlayer(playername);

            if (p.state != (sbyte)Player_States.readytoplay) return null;

            Room r = _dm.Rs.GetRoom(p.roomid);
            var ships = new List<SendShips>();

            sbyte singledesk = 1, doubledesk = 1, tripledesk = 1;
                for (int y = 0; y < p.field.Length; y++)
                {
                    for (int x = 0; x < p.field.Length; x++)
                    {
                        if(p.field[y][x]==(sbyte)Field_Cell_States.ship)
                        {
                            bool fl = true;
                            if(x-1>=0&&p.field[y][x-1] == (sbyte)Field_Cell_States.ship)
                                fl = false;
                            else
                            if (y-1>=0&&p.field[y-1][x] == (sbyte)Field_Cell_States.ship)
                                fl = false;


                            if(fl)
                            {
                                sbyte deskcount = 1;
                                char align='n';
                                int i = x+1;
                                while (i < p.field.Length && p.field[y][i] == (sbyte)Field_Cell_States.ship)
                                {
                                    align = 'h';
                                    deskcount++;
                                    i++;
                                }

                                if (align != 'h')
                                {
                                    i = y + 1;
                                    while (i < p.field.Length && p.field[i][x] == (sbyte)Field_Cell_States.ship)
                                    {
                                        align = 'v';
                                        deskcount++;
                                        i++;
                                    }
                                }

                                SendShips s = new SendShips();

                                s.x = (sbyte)x;
                                s.y = (sbyte)y;
                                s.align = align;
                                switch(deskcount)
                                {
                                    case 1: s.shipname = "singledesk" + singledesk.ToString(); singledesk++; break;
                                    case 2: s.shipname = "doubledesk" + doubledesk.ToString(); doubledesk++; ; break;
                                    case 3: s.shipname = "tripledesk" + tripledesk.ToString(); tripledesk++; ; break;
                                    case 4: s.shipname = "fourdesk";  break;
                                }
                              ships.Add(s);

                            }

                        }

                    }
                }

            



            return ships;
        }


        public Dictionary<string, string> UpdateRoom(string playername)
        {

            Player p1 = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(p1.roomid);
            Player p2 = _dm.Rs.GetPlayer2(p1,r);
            string p2res = "";
            string p2name = "";
            if (p2 != null) p2name = p2.login;

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
                          p2res = "Поиск соперника";
                          return true;
                      }

                  }
                  else return false;

              };


            Func<string> waitingplayer = () =>
            {
                p2res = "Поиск соперника";
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
                            p2res ="Готов";

                            return "";
                        }
                        else
                        {
                            p2res = "Готов";
                            return "";
                        }
                    }
                    else
                    {
                        p2res = "Готовимся к бою";
                        return "";
                    }

            };

            Func<string> readytoplay = () =>
            {
                    p2res = "Готов";
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
                p2res = "Поиск соперника";
                return "";
            };
            var gamestates = new Dictionary<sbyte, Func<string>>(4);
            gamestates.Add((sbyte)Game_States.waitingplayer, waitingplayer);
            gamestates.Add((sbyte)Game_States.editships, editships);
            gamestates.Add((sbyte)Game_States.readytoplay, readytoplay);
            gamestates.Add((sbyte)Game_States.readytoreplay, readytoreplay);
            r.updTime = DateTime.Now;

            var res = new Dictionary<string, string>(3);

            res.Add("gamestatus", gamestates[r.status]());
            res.Add("player2name", p2name);
            res.Add("player2status", p2res);
            return res;

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
