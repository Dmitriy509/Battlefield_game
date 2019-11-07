using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;
namespace BL.Interfaces
{
    public interface ISetShipsService
    {

        string CheckGameState(string playername);
        Dictionary<string, string> UpdateRoom(string playername);
        bool GetCoords(string playername, int[] Xarr, int[] Yarr);
        void LeaveRoom(string playername);
        List<SendShips> CheckPlayerReady(string playername);
    }

}
