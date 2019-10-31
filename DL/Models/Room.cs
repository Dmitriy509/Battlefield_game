using System;
using System.Collections.Generic;
using System.Text;
using static DL.Enums.StateEnums;
namespace DL.Models
{
    public class Room
    {
        public uint id;
        public string Name;
        public uint? player1id;
        public uint? player2id;
        public DateTime updTime;
        public sbyte status = -1;
        public sbyte movepriority;
        public bool flUpdateField;

    }
}
