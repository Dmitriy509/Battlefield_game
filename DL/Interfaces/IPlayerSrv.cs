using System;
using System.Collections.Generic;
using System.Text;
using DL.Models;

namespace DL.Interfaces
{
    public interface IPlayerSrv
    {
        void InitPlayer(Player player, bool froom = true, bool fstate = true);

        List<Player> GetAllPlayers();
        Player GetPlayer(string name, bool update = false);
        bool DeletePlayer(string name);
        bool DeletePlayer(Player p);
        bool checkPlayer(string name);
        bool AddPlayer(string name);
        void UpdatePlayer(Player player);

    }
}
