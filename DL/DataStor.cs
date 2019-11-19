using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DL.Models;
namespace DL
{
    public class DataStor
    {
        static ConcurrentDictionary<uint,Room> _rooms = new ConcurrentDictionary<uint,Room>();
        static ConcurrentDictionary<uint,Player> _players = new ConcurrentDictionary<uint,Player>();
        static uint _playersid=0;
        static uint _roomsid = 0;

        //public List<Player> Players { get; set; }
        public ConcurrentDictionary<uint,Player> Players { get { return _players; } }
        public ConcurrentDictionary<uint, Room> Rooms { get { return _rooms; } }
        public uint GetPlayersId { get { _playersid++;  return _playersid; } }
        public uint GetRoomsId { get { _roomsid++; return _roomsid; } }
    }
}
