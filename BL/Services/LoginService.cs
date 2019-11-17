using System;
using System.Collections.Generic;
using System.Text;
using DL.Models;
using BL.Interfaces;



namespace BL.Services
{
    public class LoginService: commonSrv, ILoginService
    {
        
        public LoginService()
        {
           
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

        public bool SignOut(string playername)
        {
           return _dm.Ps.DeletePlayer(_dm.Ps.GetPlayer(playername));                 
        }




    }
}
