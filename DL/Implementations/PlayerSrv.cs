using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DL.Models;
using DL.Interfaces;
using static DL.Enums.StateEnums;

namespace DL.Implementations
{
    public class PlaerSrv: IPlayerSrv
    {
        DataStor _ds;
        public PlaerSrv(DataStor ds)
        {
            _ds = ds;
        }

        public void InitPlayer(Player player, bool froom = true, bool fstate=true)
        {

          if(froom)  player.room = null;
          if(fstate)  player.state = (sbyte)Player_States.signin;
            player.date = DateTime.Now;

            for (int i = 0; i < 10; i++)
            {
                player.field[i] = new sbyte[10];
                for (int j = 0; j < 10; j++)
                    player.field[i][j] = (sbyte)Field_Cell_States.empty;
            }

        }
        public List<Player> GetAllPlayers()
        {
            return _ds.Players;
        }

        public Player GetPlayer(string name, bool update = false)
        {
            Player p = _ds.Players.FirstOrDefault(u => u.login == name);
            if (p == null) return null;
            if (update) p.date = DateTime.Now;
            return p;
        }
        public bool DeletePlayer(string name)
        {
            Player p = _ds.Players.FirstOrDefault(u => u.login == name);
            return _ds.Players.Remove(p);
        }
        public bool DeletePlayer(Player p)
        {
            return _ds.Players.Remove(p);
        }
     
        public bool checkPlayer(string name)
        {
            
            //  int a = DataStor._players.Count;
            if (_ds.Players.Count == 0) return false;
            var items = _ds.Players.Where(u => u.login == name);
            bool flPlayerExist = false;
            foreach (var pl in items)
            {
                TimeSpan ts = DateTime.Now - pl.date;
                if (ts.TotalMinutes > Parameters.KeepLoginCokies)
                    _ds.Players.Remove(pl);
                else flPlayerExist = true;
            }
            if (flPlayerExist)
                return true;
            else return false;
        }

        public bool AddPlayer(string name)
        {
            if (checkPlayer(name)) return false;
            Player p = new Player();
            p.login = name;
            p.date = DateTime.Now;
            // DataStor._players.Add(p);
            _ds.Players.Add(p);
            return true;
        }

        public void UpdatePlayer(Player player)
        {

        }


    }
}
