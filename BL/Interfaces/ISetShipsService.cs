using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;
namespace BL.Interfaces
{
    public interface ISetShipsService
    {

        string CheckGameState(string player_id);
        Dictionary<string, string> UpdateRoom(string player_id);
        bool GetCoords(string player_id, int[] Xarr, int[] Yarr);
        void LeaveRoom(string player_id);
        List<SendShips> CheckPlayerReady(string player_id);
    }

}
