using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using DL.Models;
using BL.Interfaces;
using DL.Enums;


namespace BL.Services
{
    public class LoginService: commonSrv, ILoginService
    {
     
        public LoginService(ILogger logger):base(logger)
        {
           
        }

        public string Login(string player_id)
        {
            Player p = _dm.Ps.GetPlayer(convertId(player_id));
            if (p!=null)
            {

                if(getInterval(p.date, Parameters.deletePlaeyrTimeout, '>')) return "";

                return LoginStateMachine(p);
            }

            return "";
        }


        public string SignIn(string playername, out uint player_id)
        {

            if (_dm.Ps.AddPlayer(playername))
            {
                player_id= _dm.Ps.GetPlayer(playername).id;
                return "Rooms";
            }

            player_id = 0;
            return "Login";  
        }

        public bool SignOut(string player_id)
        {
           return _dm.Ps.DeletePlayer(_dm.Ps.GetPlayer(convertId(player_id)));                 
        }




    }
}
