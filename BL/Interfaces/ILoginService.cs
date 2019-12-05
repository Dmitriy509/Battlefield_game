using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
   public interface ILoginService
    {
         string Login(string player_id);
         string SignIn(string playername, out uint player_id);
         bool SignOut(string player_id);
    }
}
