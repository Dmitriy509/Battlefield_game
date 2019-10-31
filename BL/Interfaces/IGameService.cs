using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;

namespace BL.Interfaces
{
    public interface IGameService
    {
        string CheckGameState(string playername);
        StartGameData InitGame(string playername);
        string StartGame(string playername);
        string GetPlayer2name(string playername);
        GameProcessData GameProcessStateMachine(string playername, sbyte curmovestate);
        FireResults Fire(string playername, int x, int y);
        void GiveUp(string playername);


    }
}
