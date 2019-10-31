using System;
using System.Collections.Generic;
using DL.Models;
namespace DL
{
    public class DataStor
    {
        static Dictionary<uint,Room> _rooms = new Dictionary<uint,Room>();
        static Dictionary<uint,Player> _players = new Dictionary<uint,Player>();
        static uint _playersid=0;
        static uint _roomsid = 0;

        //public List<Player> Players { get; set; }
        public Dictionary<uint,Player> Players { get { return _players; } }
        public Dictionary<uint, Room> Rooms { get { return _rooms; } }
        public uint GetPlayersId { get { _playersid++;  return _playersid; } }
        public uint GetRoomsId { get { _roomsid++; return _roomsid; } }
    }
}
