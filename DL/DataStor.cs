using System;
using System.Collections.Generic;
using DL.Models;
namespace DL
{
    public class DataStor
    {
        static List<Room> _rooms = new List<Room>();
        static List<Player> _players = new List<Player>();

        //public List<Player> Players { get; set; }
        public List<Player> Players { get { return _players; } }
        public List<Room> Rooms { get { return _rooms; } }
    }
}
