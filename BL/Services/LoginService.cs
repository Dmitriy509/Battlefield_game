using System;
using System.Collections.Generic;
using System.Text;
using DL;
using DL.Models;
using BL.Interfaces;
using static DL.Enums.StateEnums;

namespace BL.Services
{
    public class LoginService: commonSrv, ILoginService
    {
        DataManager _dm;
        public LoginService()
        {
            _dm = new DataManager();
        }

        public string Login(string playername)
        {
            if (_dm.Ps.checkPlayer(playername))
            {
                Player p = _dm.Ps.GetPlayer(playername);
                //    return View(_dm.Sts.loginview(p, _gamesrv));
                return LoginStateMachine(p);
            }

            return "";
        }


        
        //bool checkRoom(Room r, Player p)
        //{
        //    if (r != null)
        //    {
        //        if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
        //        {
        //            _dm.Rs.DeleteRoom(r);
        //            _dm.Ps.InitPlayer(p);
        //            return false;
        //        }
        //        return true;
        //    }

        //    return false;
        //}

        private string LoginStateMachine(Player p)
        {
            Room r = p.room;
            Func<string> signin = () =>
            { 
                if(r!=null)
                {        
                    if(getInterval(r.updTime, Parameters.PlayerDisconnect, '>'))
                    { 
                        _dm.Rs.DeleteRoom(r);
                    }
                }
                _dm.Ps.InitPlayer(p);
                return "~/Rooms/Rooms";
            };

            Func<string> editships = () =>
            {

                if (r != null)
                {

                  if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                    if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                }
                else
                {
                    _dm.Ps.InitPlayer(p);
                    return "~/Rooms/Rooms";
                }

                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.editships: break;
                    case (sbyte)Game_States.waitingplayer: break;
                    case (sbyte)Game_States.readytoreplay: break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms"; 
                }



                return "~/SetShips/FieldEditorView";
            };

            Func<string> readytoplay = () =>
            {

                if (r != null)
                {

                    if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                    if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                }
                else
                {
                    _dm.Ps.InitPlayer(p);
                    return "~/Rooms/Rooms";
                }


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.readytoplay: break;
                    case (sbyte)Game_States.playerdisconnected:break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }

                return "~/Game/StartGame";
            };
       
            Func<string> playing = () =>
            {
                if (r != null)
                {

                    if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                    if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                }
                else
                {
                    _dm.Ps.InitPlayer(p);
                    return "~/Rooms/Rooms";
                }


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.playing: break;
                    case (sbyte)Game_States.playerdisconnected: break;
                    case (sbyte)Game_States.readytoplay: break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }

                return "~/Game/GameView";
            };

            Func<string> readytoreplay = () =>
            {
                if (r != null)
                {

                    if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                    if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                }
                else
                {
                    _dm.Ps.InitPlayer(p);
                    return "~/Rooms/Rooms";
                }


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.readytoreplay:
                        //editships
                        break;
                    case (sbyte)Game_States.endofgame:
                        //go to endofgame
                        break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }



                return "~/SetShips/FieldEditorView";
            };

            Func<string> winner = () =>
            {
                if (r != null)
                {

                    if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                    if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                    {
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                    }

                }
                else
                {
                    _dm.Ps.InitPlayer(p);
                    return "~/Rooms/Rooms";
                }


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.endofgame:
                    
                        break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }


                return "~/SetShips/FieldEditorView";
      
            };

            Func<string> loser = () =>
            {
                return winner();

            };

            Func<string> giveup = () =>
            {
                return winner();

            };


            var playerstates = new Dictionary<sbyte, Func<string>>(4);
            playerstates.Add((sbyte)Player_States.signin, signin);
            playerstates.Add((sbyte)Player_States.editships, editships);
            playerstates.Add((sbyte)Player_States.readytoplay, editships);
            playerstates.Add((sbyte)Player_States.playing, playing);
            playerstates.Add((sbyte)Player_States.readytoreplay, readytoreplay);
            playerstates.Add((sbyte)Player_States.winner, winner);
            playerstates.Add((sbyte)Player_States.loser, loser);
            playerstates.Add((sbyte)Player_States.giveup, giveup);
            return playerstates[p.state]();

   
        }

        public string SignIn(string playername)
        {
            //   bool fl = _gamesrv.Ps.AddPlayer(username);
            if (_dm.Ps.AddPlayer(playername))
            {
              //  addCookies(username);
              //  ViewBag.playername = username;
                return "Rooms";
            }

            //ViewBag.errormsg = "Игрок с таким именем уже есть на сервере!";
            return "Login";

    
        }


    }
}
