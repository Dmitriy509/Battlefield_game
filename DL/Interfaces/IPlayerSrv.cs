using System;
using System.Collections.Generic;
using System.Text;
using DL.Models;

namespace DL.Interfaces
{
    public interface IPlayerSrv
    {
        void InitPlayer(Player player, bool froom = true, bool fstate = true);

        IEnumerable<Player> GetAllPlayers();
        Player GetPlayer(string name, bool update = false);
        Player GetPlayer(uint? id, bool update = false);
        bool DeletePlayerId(uint id);
        bool DeletePlayer(Player p);
        bool checkPlayer(string name);
        bool AddPlayer(string name);
        void UpdatePlayer(Player player);

    }
}
