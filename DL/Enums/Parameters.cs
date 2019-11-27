using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Enums
{
    public class Parameters
    {
        public const int KeepLoginCokies = 720; //min
        public const int deletePlaeyrTimeout = 1; //min
        public const int PlayerDisconnect = 10; //sek
        public const int WaitReconnect = PlayerDisconnect+5; //sek
        public const int WaitReplayGame = 15; //sek
        public const int MoveTime = 30; //sek
        //public const int PlayerDisconnect = 10; //sek
    }
}
