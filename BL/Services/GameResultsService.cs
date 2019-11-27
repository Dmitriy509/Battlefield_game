using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Models;
using DL;
using DL.Models;
using static DL.Enums.StateEnums;
namespace BL.Services
{
    public class GameResultsService: commonSrv, IGameResultsService
    {

        public GameResultsService(ILogger logger) : base(logger)
        {

        }

        public string updateGameResult(string player_id, bool fltimeisup)
        {
            Player player = _dm.Ps.GetPlayer(convertId(player_id), true);


            Room room = _dm.Rs.GetRoom(player.roomid);
            Player player2;


            if (fltimeisup || room == null)
            {
                if (room != null)
                {
                    player2 = _dm.Rs.GetPlayer2(player,room);
                    if (player2 != null) _dm.Ps.InitPlayer(player2);
                    _dm.Rs.DeleteRoom(room);
                    _logger.LogInformation("Player_Id: " + player.id + ", Room_Id: " + player.roomid + ", waiting replay time is up, room was deleted");
                }
                _dm.Ps.InitPlayer(player);
                return "exit"; 
            }
            else
            {
                if((player.id==room.player1id&& room.player2id==null)|| (player.id == room.player2id && room.player1id == null))
                {              
                    _dm.Rs.DeleteRoom(room);
                    _dm.Ps.InitPlayer(player);
                    return "exit";
                }
            }

            room.updTime = DateTime.Now;
            string res = "wait";

            player2 = _dm.Rs.GetPlayer2(player, room);
            switch (player.state)
            {
                case (sbyte)Player_States.signin:

                    break;

                case (sbyte)Player_States.winner:
                case (sbyte)Player_States.giveup:
                case (sbyte)Player_States.loser:
                   
                    if (player2.state == (sbyte)Player_States.readytoreplay)                     
                        return "ready";
                    //goto case (sbyte)Player_States.readytoreplay;
                    break;
                case (sbyte)Player_States.readytoreplay:
                    
                    if (player2.state == (sbyte)Player_States.readytoreplay)
                    {
                        room.status = (sbyte)Game_States.readytoreplay;
                        return "ready";
                    }
                    if (room.status == (sbyte)Game_States.readytoreplay)
                        return "ready";
                    break;

            }

          //  if(room.status == (sbyte)Game_States.readytoreplay) return "ready";

            return res;
        }

        public void replayGame(string player_id)
        {

        
            Player p1 = _dm.Ps.GetPlayer(convertId(player_id), true);
            p1.state = (sbyte)Player_States.readytoreplay;
            _dm.Ps.InitPlayer(p1, false, false);
            _logger.LogInformation("Player_Id: " + p1.id + ", Room_Id: " + (p1.roomid==null?"-":p1.roomid.ToString()) + ", Player ready to replay ");


        }

        public void exitGame(string player_id)
        {
            Player p1 = _dm.Ps.GetPlayer(convertId(player_id), true);

            if (p1.roomid != null)
            {
                Room r = _dm.Rs.GetRoom(p1.roomid);
                if (r != null)
                {
                    Player p2 = _dm.Rs.GetPlayer2(p1, r);
                    if (p2 == null)
                    {
                        //  _dm.Ps.InitPlayer(p1);
                        _dm.Rs.DeleteRoom(r);
                    }
                    else
                    {
                        _dm.Ps.InitPlayer(p2);
                        // _dm.Ps.InitPlayer(p1);
                        _dm.Rs.DeleteRoom(r);
                    }
                    _logger.LogInformation("Player_Id: " + p1.id+", Room_Id: " + p1.roomid + ", Player left the room, room was deleted ");
                }
            }
            else
              _logger.LogInformation("Player_Id: " + p1.id + ", Player left the room");


            _dm.Ps.InitPlayer(p1);


        }


        public void updatePlayer(string player_id)
        {
            _dm.Ps.GetPlayer(player_id, true);
        }

    }
}
