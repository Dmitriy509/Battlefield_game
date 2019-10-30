using System;
using System.Collections.Generic;
using System.Text;
using BL.Interfaces;
using BL.Models;
using DL;
using DL.Models;
using static DL.Enums.StateEnums;
namespace BL.Services
{
    public class GameResultsService:IGameResultsService
    {
        DataManager _dm;
        public GameResultsService()
        {
            _dm = new DataManager();
        }

        public string updateGameResult(string playername, bool fltimeisup)
        {
            Player player = _dm.Ps.GetPlayer(playername, true);


            Room room = player.room;



            if (fltimeisup || room == null)
            {
                if (room != null)
                {
                    Player player2 = _dm.Rs.GetPlayer2(player);
                    if (player2 != null) _dm.Ps.InitPlayer(player2);
                    _dm.Rs.DeleteRoom(room);
                }
                _dm.Ps.InitPlayer(player);
                return "Игрок вышел";
            }

            string res = "Ожидаем";
            switch (player.state)
            {
                case (sbyte)Player_States.signin:

                    break;
                case (sbyte)Player_States.winner:
                case (sbyte)Player_States.giveup:
                case (sbyte)Player_States.loser:
                //if (playerready == true)
                //{
                //    player.state = (sbyte)Player_States.readytoreplay;
                //}
                //goto case (sbyte)Player_States.readytoreplay;
                case (sbyte)Player_States.readytoreplay:
                    Player player2 = _dm.Rs.GetPlayer2(player);
                    if (player2.state == (sbyte)Player_States.readytoreplay)
                    {
                        room.status = (sbyte)Game_States.readytoreplay;
                        return "Готов";
                    }
                    break;

            }
            return res;
        }

        public void replayGame(string playername)
        {

            Player p1 = _dm.Ps.GetPlayer(playername, true);
            _dm.Ps.InitPlayer(p1, false, false);
            p1.state = (sbyte)Player_States.readytoreplay;


        }

        public void exitGame(string playername)
        {
            Player p1 = _dm.Ps.GetPlayer(playername, true);
        
            if (p1.room == null)
            {
                _dm.Ps.InitPlayer(p1);
            }
            else
            {
                Room r = p1.room;
                Player p2 = _dm.Rs.GetPlayer2(p1);
                if (p2 == null)
                {
                    _dm.Ps.InitPlayer(p1);
                    _dm.Rs.DeleteRoom(r);
                }
                else
                {
                    _dm.Ps.InitPlayer(p2);
                    _dm.Ps.InitPlayer(p1);
                    _dm.Rs.DeleteRoom(r);
                }

            }

        }

    }
}
