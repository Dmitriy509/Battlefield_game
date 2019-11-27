using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;

namespace BL.Interfaces
{
    public interface IGameService
    {
        string CheckGameState(string player_id);
        StartGameData InitGame(string player_id);
        bool StartGame(string player_id);
     //   string GetPlayer2name(string playername);
        GameProcessData GameProcessStateMachine(string player_id, sbyte curmovestate);
        FireResults Fire(string player_id, int x, int y);
        void GiveUp(string player_id);


    }
}
