using System;
using System.Collections.Generic;
using System.Text;
using DL;
using DL.Models;
using BL.Interfaces;
using static DL.Enums.StateEnums;
using DL.Enums;

namespace BL.Services
{
    public class commonSrv
    {
        protected DataManager _dm;
        public commonSrv()
        {
            _dm = new DataManager();
        }

        public bool getInterval(DateTime d, int sec, char sign)
        {
            TimeSpan ts = DateTime.Now - d;
            if(sign=='>')
            return ts.TotalSeconds > sec;
            else
            if (sign == '<')
                return ts.TotalSeconds < sec;

            return false;
        }

        bool checkRoom(Room r, Player p)
        {
            if (r != null)
            {

                if (getInterval(r.updTime, Parameters.WaitReconnect, '>'))
                {
                    _dm.Rs.DeleteRoom(r);
                    _dm.Ps.InitPlayer(p);
                    return false;
                }

                if (getInterval(p.date, Parameters.WaitReconnect, '>'))
                {
                    _dm.Ps.InitPlayer(p);
                    return false;
                }

            }
            else
            {
                _dm.Ps.InitPlayer(p);
                return false;
            }

            return true;
        }

        protected string LoginStateMachine(Player p)
        {
            Room r = _dm.Rs.GetRoom(p.roomid);
            Func<string> signin = () =>
            {
                if (r != null)
                {
                    if (getInterval(r.updTime, Parameters.PlayerDisconnect, '>'))
                    {
                        _dm.Rs.DeleteRoom(r);
                    }
                }
                _dm.Ps.InitPlayer(p);
                return "~/Rooms/Rooms";
            };

            Func<string> editships = () =>
            {

                if (!checkRoom(r, p)) return "~/Rooms/Rooms";

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

                if (!checkRoom(r, p)) return "~/Rooms/Rooms";


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.readytoplay: break;
                    case (sbyte)Game_States.playerdisconnected: break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }

                return "~/Game/StartGame";
            };

            Func<string> playing = () =>
            {
                if (!checkRoom(r, p)) return "~/Rooms/Rooms";


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
                if (!checkRoom(r, p)) return "~/Rooms/Rooms";


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
                if (!checkRoom(r, p)) return "~/Rooms/Rooms";


                switch (r.status) //valid game states
                {
                    case (sbyte)Game_States.endofgame:

                        break;
                    default:
                        _dm.Ps.InitPlayer(p);
                        return "~/Rooms/Rooms";
                }


                return "~/Game/GameView";

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


    }
}
