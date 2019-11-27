using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;

namespace BL.Interfaces
{
    public interface IGameResultsService
    {
        string updateGameResult(string player_id, bool timeisup);
        void replayGame(string player_id);
        void exitGame(string player_id);
        void updatePlayer(string player_id);
    }
}
