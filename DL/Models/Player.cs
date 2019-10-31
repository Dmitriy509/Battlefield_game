using System;
using System.Collections.Generic;
using System.Text;
using static  DL.Enums.StateEnums;

namespace DL.Models
{
    public class Player
    {
        public uint id;
        public string login;
        public uint? roomid;
        public sbyte[][] field;//0-туман, 1-пусто, 2-корабль, 3-раненый, 4-промах
        public sbyte state; //0 -авторизовался, 1 - расставляет корабли, 2 - готов, 3 - бой
        public DateTime date;
        public sbyte[] shipcount = { 0, 0, 0, 0 };

        public Player()
        {
          
            state = (sbyte)Player_States.signin;
            field = new sbyte[10][];
            for (int i = 0; i < 10; i++)
            {
                field[i] = new sbyte[10];
                for (int j = 0; j < 10; j++)
                    field[i][j] = (sbyte)Field_Cell_States.empty;
            }
        }


    }
}
