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
            Room p = _ds.Rooms.FirstOrDefault(u => u.Value.Name == name).Value;
            return p;
        }
        public Room GetRoom(uint? idroom)
        {
            if (idroom == null) return null;
            if (_ds.Rooms.ContainsKey((uint)idroom))
                return _ds.Rooms[(uint)idroom];
            else return null;
        }
        public IEnumerable<Room> GetAllRooms()
        {

            return _ds.Rooms.Select(u=>u.Value);
        }


        public bool DeleteRoom(Room r)
        {
            return _ds.Rooms.Remove(r.id);
        }
        public Room AddRoom(string name)
        {
            if (GetRoom(name) == null)
            {
                Room r = new Room();
                r.Name = name;
                r.updTime = DateTime.Now;
                r.movepriority = (sbyte)Moves_States.undefined;
                r.player1id = null;
                r.player2id = null;
                r.id = _ds.GetRoomsId;
                _ds.Rooms.Add(r.id, r);
                return r;
            }

            return null;

        }

        public bool AddPlayer(Player p, Room r)
        {

            if (r.player1id== null) r.player1id = p.id;
            else
                if (r.player2id == null)
                r.player2id = p.id;
            else return false;
            r.updTime = DateTime.Now;
            return true;
        }
        public Player GetPlayer2(Player player1, Room r)
        {
            if(r.player1id==null|| r.player2id == null) return null;
            if (player1.id==r.player1id)
            {             
                    return _ds.Players[(uint)r.player2id];             
            }
            else
            {
                    return _ds.Players[(uint)r.player1id];
            }
        }

        public void UpdateRoom(Room room)
        {

        }


    }
}
