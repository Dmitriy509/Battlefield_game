using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;

namespace BL.Interfaces
{
    public interface IGameResultsService
    {
        string updateGameResult(string playername, bool timeisup);
        void replayGame(string playername);
        void exitGame(string playername);
        void updatePlayer(string playername);
    }
}
