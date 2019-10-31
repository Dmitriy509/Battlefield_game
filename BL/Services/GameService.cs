using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BL.Interfaces;
using BL.Models;
using DL.Enums;
using DL;
using DL.Models;
using static DL.Enums.StateEnums;

namespace BL.Services
{
    public class GameService : commonSrv, IGameService
    {

        public GameService()
        {

        }

        public string CheckGameState(string playername)
        {

            Player p = _dm.Ps.GetPlayer(playername);
            if (p == null) return "~/Login/Login";
            return LoginStateMachine(p);

        }


        public string StartGame(string playername)
        {

            //ViewBag.coordsField = _gamesrv.Ps.GetPlayer(playername).field;
            ////(sbyte)Moves_States.undefined;
            Player player = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(player.roomid);
            Player player2 = _dm.Rs.GetPlayer2(player, r);

            for (sbyte i = 0; i < 4; i++) player.shipcount[i] = (sbyte)(4 - i);
            r.updTime = DateTime.Now;

            player.state = (sbyte)Player_States.playing;

            if (player.state == player2.state)
                r.status = (sbyte)Game_States.playing;
           
            if (player.id == r.player1id) //determine who goes first
                if (r.movepriority == (sbyte)Moves_States.undefined)
                {
                    if (DateTime.Now.Second > 30)
                        r.movepriority = (sbyte)Moves_States.player1;
                    else r.movepriority = (sbyte)Moves_States.player2;
                }

            return _dm.Rs.GetPlayer2(player,r).login;
            //ViewBag.player1name = playername;
            //ViewBag.player2name = _gamesrv.Rs.GetPlayer2(player).login;

        }


        public StartGameData InitGame(string playername)
        {
            StartGameData res = new StartGameData();
            Player player = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(player.roomid);
            Player player2 = _dm.Rs.GetPlayer2(player, r);
            res.player2name = player2.login;
            res.player1name = playername;
            res.player1field = player.field;
            res.player2field = player2.field;

            return res;
        }



        public string GetPlayer2name(string playername)
        {
            Player player = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(player.roomid);
            return _dm.Rs.GetPlayer2(player, r).login;
        }



        public GameProcessData GameProcessStateMachine(string playername, sbyte curmovestate)
        {
            GameProcessData res = new GameProcessData();
            res.curmovestate = curmovestate;
            Player p1 = _dm.Ps.GetPlayer(playername, true);
            Room r = _dm.Rs.GetRoom(p1.roomid);
            if (r != null) r.updTime = DateTime.Now;
            Player p2 = _dm.Rs.GetPlayer2(p1,r);
            



            Action playermove = () =>
            {
                if ((p1.id == r.player1id && r.movepriority == (sbyte)Moves_States.player1) || (p1.id == r.player2id && r.movepriority == (sbyte)Moves_States.player2))
                {

                    if (res.curmovestate == 2)
                    {
                        res.PlayerField = p1.field;
                        res.shipscount = p1.shipcount;
                        res.curmovestate = 1;
                    }
                    //   else sendfied = new sbyte[][];
                }
                else
                {
                    if (r.flUpdateField)
                    {
                        res.PlayerField = p1.field;
                        res.shipscount = p1.shipcount;
                        r.flUpdateField = false;
                    }
                    res.curmovestate = 2;
                }

            };

            Func<string> endofgame = () =>
            {

                if (p1.state == (sbyte)Player_States.playing)
                {
                    switch (p2.state)
                    {
                        case (sbyte)Player_States.playing:
                            {
                                if (p1.shipcount.Sum(u => u) == 0)
                                {
                                    p1.state = (sbyte)Player_States.loser;
                                    p2.state = (sbyte)Player_States.winner;
                                }

                                else
                                {
                                    p1.state = (sbyte)Player_States.winner;
                                    p2.state = (sbyte)Player_States.loser;
                                }
                                break;
                            }
                        case (sbyte)Player_States.giveup:
                        case (sbyte)Player_States.loser:
                            p1.state = (sbyte)Player_States.winner;
                            break;
                        case (sbyte)Player_States.winner:
                            p1.state = (sbyte)Player_States.loser;
                            break;
                    }

                }

                if (p1.state == (sbyte)Player_States.winner) res.curmovestate = (sbyte)Moves_States.winner;
                else res.curmovestate = (sbyte)Moves_States.loser;

                res.player2status = "";

                //  res.player2status = "";
                //   res.curmovestate = (sbyte)Moves_States.undefined;
                // resultsofgame();

                return "results";

            };
            Func<string> playing = () =>
            {
                if (getInterval(p2.date, Parameters.PlayerDisconnect, '>'))
                {
                    res.player2status = "Игрок отключился ожидаем подключения";
                    r.status = (sbyte)Game_States.playerdisconnected;
                    return "";
                }

                if (p1.shipcount.Sum(u => u) == 0 || p2.shipcount.Sum(u => u) == 0 || p1.state == (sbyte)Player_States.giveup || p2.state == (sbyte)Player_States.giveup)
                {
                    // r.status = (sbyte)Game_States.endofgame;
                    r.status = (sbyte)Game_States.endofgame;
                    return endofgame();

                }

                playermove();
                res.player2status = "";
                return "";
            };
            Func<string> playerdisconnect = () =>
            {
          
               
                if (getInterval(p2.date, Parameters.PlayerDisconnect, '>'))
                {
                    if (getInterval(p2.date, Parameters.WaitReconnect, '<'))
                    {
                        res.player2status = "Игрок отключился ожидаем подключения";
                        return "";
                    }
                    else
                    {
                        r.status = (sbyte)Game_States.endofgame;
                        if (p2.id == r.player1id) r.player1id = null; else r.player2id = null;
                            _dm.Ps.InitPlayer(p2);
                       
                        p1.state = (sbyte)Player_States.winner;


                        return endofgame();
                    }
                };
                if (p2.state == (sbyte)Player_States.readytoplay)
                {
                    p1.state = (sbyte)Player_States.playing;
                    p2.state = (sbyte)Player_States.playing;
                }

                res.player2status = "";
                r.status = (sbyte)Game_States.playing;
                return "";
            };

            Func<string> readytoplay = () =>
            {
                if (getInterval(p2.date, Parameters.PlayerDisconnect, '>'))
                {
                    res.player2status = "Игрок отключился ожидаем подключения";
                    r.status = (sbyte)Game_States.playerdisconnected;
                    return "";
                }
                

                //if (p1.state == (sbyte)Player_States.readytoplay)
                //    p1.state = (sbyte)Player_States.playing;

          //      if (p1.state == (sbyte)Player_States.playing && p2.state == (sbyte)Player_States.playing)
                 //   return playing();


                res.player2status = "Ожидаем игрока";
                return "";
            };

            var gamestates = new Dictionary<sbyte, Func<string>>(4);
            gamestates.Add((sbyte)Game_States.readytoplay, readytoplay);
            gamestates.Add((sbyte)Game_States.playing, playing);
            gamestates.Add((sbyte)Game_States.endofgame, endofgame);
            gamestates.Add((sbyte)Game_States.playerdisconnected, playerdisconnect);
            //    gamestates.Add((sbyte)Game_States.resultsofgame, resultsofgame);
            res.gamestatus = gamestates[(sbyte)r.status]();

            return res;

        }

        void getFireResults(sbyte[][] f, sbyte[] shipcount, int x, int y, FireResults res)
        {
            f[y][x] = (sbyte)Field_Cell_States.injured;

            Func<int, int, bool> compare = (u, v) =>
            {
                if (u < 0 || v < 0 || v >= f[x].Length || u >= f.Length) return false;
                return (f[u][v] == (sbyte)Field_Cell_States.ship ||
                  f[u][v] == (sbyte)Field_Cell_States.injured);
            };

            int l = x - 1, r = x + 1, t = y - 1, b = y + 1;
            while (true)
            {
                if (compare(y, l)) l--;
                else
                if (compare(y, r)) r++;
                else
                if (compare(t, x)) t--;
                else
                if (compare(b, x)) b++;
                else
                {
                    l++; r--; t++; b--;
                    break;
                }
            }

            int xk = r;
            int xn = l;

            bool flagKilled = true;
            if (xk - xn != 0)
            {
                for (int i = xn; i <= xk; i++)
                {
                    if ((sbyte)Field_Cell_States.ship == f[y][i])
                        flagKilled = false;
                }
                if (flagKilled)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        for (int i = xn - 1; i <= xk + 1; i++)
                        {
                            if (j < 0 || j >= f.Length) break;
                            if (i < 0 || i >= f[y].Length) continue;
                            if (i >= xn && i <= xk && j == y) continue;
                            if (f[j][i] != (sbyte)Field_Cell_States.miss)
                            {
                                res.XCoords.Add((sbyte)i); res.YCoords.Add((sbyte)j); res.FireRes.Add((sbyte)Field_Cell_States.miss);
                                f[j][i] = (sbyte)Field_Cell_States.miss;
                            }
                        }
                    }
                    shipcount[xk - xn]--;
                }
                res.XCoords.Add((sbyte)x); res.YCoords.Add((sbyte)y); res.FireRes.Add((sbyte)Field_Cell_States.injured);


                return;
            }
            int yk = b;
            int yn = t;

            //vertical ships
            //  if (yk - yn != 0)
            //  {
            flagKilled = true;
            for (int i = yn; i <= yk; i++)
            {
                if ((sbyte)Field_Cell_States.ship == f[i][x])
                    flagKilled = false;
            }
            if (flagKilled)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    for (int i = yn - 1; i <= yk + 1; i++)
                    {
                        if (j < 0 || j >= f[y].Length) break;
                        if (i < 0 || i >= f.Length) continue;
                        if (i >= yn && i <= yk && j == x) continue;
                        if (f[i][j] != (sbyte)Field_Cell_States.miss)
                        {
                            res.XCoords.Add((sbyte)j); res.YCoords.Add((sbyte)i); res.FireRes.Add((sbyte)Field_Cell_States.miss);
                            f[i][j] = (sbyte)Field_Cell_States.miss;
                        }
                    }
                }
                shipcount[yk - yn]--;
            }
            res.XCoords.Add((sbyte)x); res.YCoords.Add((sbyte)y); res.FireRes.Add((sbyte)Field_Cell_States.injured);

            //  }


        }

        public FireResults Fire(string playername, int x, int y)
        {
            Player player = _dm.Ps.GetPlayer(playername, true);
            Room room = _dm.Rs.GetRoom(player.roomid);
            Player player2 = _dm.Rs.GetPlayer2(player, room);
            string s = player.login;
            string s1 = player2.login;

            //List<sbyte> xc = new List<sbyte>();
            //List<sbyte> yc = new List<sbyte>();
            //List<sbyte> res = new List<sbyte>();
            FireResults res = new FireResults();
            if (player2.field[y][x] == (sbyte)Field_Cell_States.empty)
            {
                res.XCoords.Add((sbyte)x);
                res.YCoords.Add((sbyte)y);
                res.FireRes.Add((sbyte)Field_Cell_States.miss);
                player2.field[y][x] = (sbyte)Field_Cell_States.miss;
                if (room.movepriority == (sbyte)Moves_States.player1)
                    room.movepriority = (sbyte)Moves_States.player2;
                else
                    room.movepriority = (sbyte)Moves_States.player1;
            }
            else
            {

                getFireResults(player2.field, player2.shipcount, x, y, res);
                room.flUpdateField = true;
            }

            //// Game_State = 0;
            //// string[] parameters = _gamesrv.Sts.UpdateInfoRoom(player, player2, room);
            ////  room.updTime = DateTime.Now;
            res.p2shipscount = player2.shipcount;
            return res;
        }

        public void GiveUp(string playername)
        {
            

            Player p1 = _dm.Ps.GetPlayer(playername, true);
            p1.state = (sbyte)Player_States.giveup;
            Room r = _dm.Rs.GetRoom(p1.roomid);
            r.status = (sbyte)Game_States.endofgame;
            Player p2 = _dm.Rs.GetPlayer2(p1,r);
            p2.state = (sbyte)Player_States.winner;
        }
        }
}
