using System;
using System.Collections.Generic;
using System.Text;
using BL.Models;
namespace BL.Interfaces
{

    public interface IRoomsService
    {
        string CheckGameState(string player_id);
        RoomsList GetInfoRooms(string player_id);
        string[] CreateRoom(string roomName, string player_id);
        string[] EnterTheRoom(string roomname, string player_id);
    }
}
