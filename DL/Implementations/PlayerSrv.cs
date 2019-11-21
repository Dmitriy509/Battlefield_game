using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DL.Models;
using DL.Interfaces;
using static DL.Enums.StateEnums;
using DL.Enums;

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

          if(froom)  player.roomid = null;
          if(fstate)  player.state = (sbyte)Player_States.signin;
            player.date = DateTime.Now;

            for (int i = 0; i < 10; i++)
            {
                player.field[i] = new sbyte[10];
                for (int j = 0; j < 10; j++)
                    player.field[i][j] = (sbyte)Field_Cell_States.empty;
            }

        }
        public IEnumerable<Player> GetAllPlayers()
        {
            return _ds.Players.Select(u=>u.Value);
        }

        public Player GetPlayer(string name, bool update = false)
        {
            Player p = _ds.Players.FirstOrDefault(u => u.Value.login == name).Value;
            if (p == null) return null;
            if (update) p.date = DateTime.Now;
            return p;
        }
        public Player GetPlayer(uint? id, bool update = false)
        {
            if (_ds.Players.ContainsKey((uint)id))
            {
                Player p = _ds.Players[(uint)id];
                if (update) p.date = DateTime.Now;
                return p;
            }
            else return null;         
        }
        public Player GetPlayer(uint id, bool update = false)
        {
            if (_ds.Players.ContainsKey(id))
            {
                Player p = _ds.Players[id];
                if (update) p.date = DateTime.Now;
                return p;
            }
            else return null;
        }

        public bool DeletePlayerId(uint id)
        {
            Player p;
            return _ds.Players.TryRemove(id, out p);
        }
        public bool DeletePlayer(Player p)
        {
            return _ds.Players.TryRemove(p.id, out p);
        }
     
        public bool checkPlayer(string name)
        {
            
            //  int a = DataStor._players.Count;
            if (_ds.Players.Count == 0) return false;
            var items = _ds.Players.Where(u => u.Value.login==name).Select(u=>u.Value).ToList();
            bool flPlayerExist = false;
            Player p;
            foreach (var pl in items)
            {
                TimeSpan ts = DateTime.Now - pl.date;

                if (ts.TotalMinutes > Parameters.deletePlaeyrTimeout)
                    _ds.Players.TryRemove(pl.id,out p);
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
            p.roomid = null;           
            p.id = _ds.GetPlayersId;
           
            return _ds.Players.TryAdd(p.id, p);
        }

        public void UpdatePlayer(Player player)
        {

        }


    }
}
