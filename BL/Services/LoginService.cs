using System;
using System.Collections.Generic;
using System.Text;
using DL;
using DL.Models;
using BL.Interfaces;
using static DL.Enums.StateEnums;

namespace BL.Services
{
    public class LoginService: ILoginService
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

        private string LoginStateMachine(Player p)
        {
            Func<string> signin = () =>
            {
                _dm.Ps.InitPlayer(p);
                return "Rooms";
            };
            Func<string> editships = () =>
            {
                TimeSpan ts = DateTime.Now - p.date;
                if (ts.TotalMinutes > Parameters.WaitReconnect)
                {
                    _dm.Ps.InitPlayer(p);
                    return "Rooms";
                }
                return "FieldEditorView";
            };
            Func<string> playing = () =>
            {
                TimeSpan ts = DateTime.Now - p.date;
                if (ts.TotalMinutes > Parameters.WaitReconnect)
                {
                    _dm.Ps.InitPlayer(p);
                    return "Rooms";
                }
                return "GameView";
            };
            Func<string> endgame = () =>
            {
 
                return "";
            };
            Func<string> readytoreplay = () =>
            {

                return "";
            };

            var playerstates = new Dictionary<sbyte, Func<string>>(4);
            playerstates.Add((sbyte)Player_States.signin, signin);
            playerstates.Add((sbyte)Player_States.editships, editships);
            playerstates.Add((sbyte)Player_States.readytoplay, editships);
            playerstates.Add((sbyte)Player_States.playing, playing);
            playerstates.Add((sbyte)Player_States.endgame, endgame);
            playerstates.Add((sbyte)Player_States.readytoreplay, readytoreplay);

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
