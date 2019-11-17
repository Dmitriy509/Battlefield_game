using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
   public interface ILoginService
    {
         string Login(string playername);
         string SignIn(string playername);
         bool SignOut(string playername);
    }
}
