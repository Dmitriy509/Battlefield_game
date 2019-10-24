using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DL.Models;
using DL.Interfaces;
using static DL.Enums.StateEnums;


namespace DL.Implementations
{
    public class RoomSrv: IRoomSrv
    {
        DataStor _ds;
        public RoomSrv(DataStor ds)
        {
            _ds = ds;
        }
        public Room GetRoom(string name)
        {
            Room p = _ds.Rooms.FirstOrDefault(u => u.Name == name);
            return p;
        }

        public List<Room> GetAllRooms()
        {

            return _ds.Rooms;
        }

        public bool DeleteRoom(string name)
        {
            Room p = _ds.Rooms.FirstOrDefault(u => u.Name == name);
            return _ds.Rooms.Remove(p);
        }
        public bool DeleteRoom(Room r)
        {
            return _ds.Rooms.Remove(r);
        }
        public Room AddRoom(string name)
        {
            if (GetRoom(name) == null)
            {
                Room r = new Room();
                r.Name = name;
                r.updTime = DateTime.Now;
                r.movepriority = (sbyte)Moves_States.undefined;
                _ds.Rooms.Add(r);
                return r;
            }

            return null;

        }

        public bool AddPlayer(Player p, Room r)
        {

            if (r.player1 == null) r.player1 = p;
            else
                if (r.player2 == null)
                r.player2 = p;
            else return false;
            r.updTime = DateTime.Now;
            return true;
        }
        public Player GetPlayer2(Player player1)
        {
            Room r = player1.room;
            if (r.player1 == player1) return r.player2;
            else return r.player1;
        }

        public void UpdateRoom(Room room)
        {

        }


    }
}
