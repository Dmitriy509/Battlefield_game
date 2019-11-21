using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using DL.Models;
using BL.Interfaces;



namespace BL.Services
{
    public class LoginService: commonSrv, ILoginService
    {
        private readonly ILogger _logger;
        public LoginService(ILogger logger)
        {
            _logger = logger;
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

            if (_dm.Ps.AddPlayer(playername))
            {
                return "Rooms";
            }
       
        
            return "Login";  
        }

        public bool SignOut(string playername)
        {
           return _dm.Ps.DeletePlayer(_dm.Ps.GetPlayer(playername));                 
        }




    }
}
