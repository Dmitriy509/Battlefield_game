using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Enums
{
    public class StateEnums
    {
        public enum Player_States : sbyte
        {
            signin,
            editships,
            readytoplay,
            playing,            
            readytoreplay,
            winner,
            loser,
            giveup

        }

        public enum Game_States : sbyte
        {
            waitingplayer = 0,
            editships = 1,
            readytoplay = 2,
            playing = 3,
            endofgame = 4,
            playerdisconnected = 5
        }
        //0-туман, 1-пусто, 2-корабль, 3-раненый, 4-промах
        public enum Field_Cell_States : sbyte
        {
            empty = 1,
            ship = 2,
            injured = 3,
            miss = 4
        }

        public enum Moves_States : sbyte
        {
            player1 = 1,
            player2 = 2,
            undefined = 3
        }

    }
}
