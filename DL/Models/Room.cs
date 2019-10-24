using System;
using System.Collections.Generic;
using System.Text;
using static DL.Enums.StateEnums;
namespace DL.Models
{
    public class Room
    {
        public string Name;
        public Player player1;
        public Player player2;
        public DateTime updTime;
        public sbyte status = -1;
        public sbyte movepriority;
        public bool flUpdateField;

    }
}
