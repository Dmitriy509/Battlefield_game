using System;
using System.Collections.Generic;
using System.Text;
using DL.Models;
namespace DL.Interfaces
{
   public interface IRoomSrv
    {
        Room GetRoom(string name);
        Room GetRoom(uint? idroom);
        IEnumerable<Room> GetAllRooms();
       // bool DeleteRoom(string name);
        bool DeleteRoom(Room r);
        Room AddRoom(string name);
        bool AddPlayer(Player p, Room r);
        Player GetPlayer2(Player player1, Room r);
        void UpdateRoom(Room room);
    }
}
