using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface ISetShipsService
    {
        string[] UpdateRoom(string playername);
        bool GetCoords(string playername, int[] Xarr, int[] Yarr);
        void LeaveRoom(string playername);
    }

}
