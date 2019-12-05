using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using DL.Models;
using BL.Interfaces;
using DL.Enums;
using DL;

namespace BL
{
    class BackGroundThread
    {
        private static Thread _th= new Thread(Check);
        private static DataManager _dm = new DataManager();
        public static void Run()
        {

            if (!_th.IsAlive)
            {
                //  th = new Thread(new ThreadStart(Check));
                _th.IsBackground = true;
                _th = new Thread(Check);
                _th.Start();
            }
        }


        private static void Check()
        {

            while (true)
            {
                if (_dm.Rs.GetAllRooms().Any())
                {
                    var rooms = _dm.Rs.GetAllRooms().ToList();
                    foreach (var r in rooms)
                    {
                        TimeSpan ts = DateTime.Now - r.updTime;
                        if (ts.TotalSeconds > Parameters.WaitReconnect)
                        {
                            Player p = null;
                            Action delPlayer = () =>
                             {
                                 if (p != null)
                                 {
                                     double t = (DateTime.Now - p.date).TotalSeconds;
                                     if (t > Parameters.deletePlaeyrTimeout)
                                     {
                                         _dm.Ps.DeletePlayer(p);
                                     }
                                     else
                                      if (t > Parameters.WaitReconnect)
                                     {
                                         DateTime d = p.date;
                                         _dm.Ps.InitPlayer(p);
                                         p.date = d;
                                     }
                                 }
                             };
                       
                            p = _dm.Ps.GetPlayer(r.player1id);
                            delPlayer();
                            p = _dm.Ps.GetPlayer(r.player2id);
                            delPlayer();
                            _dm.Rs.DeleteRoom(r);
                        }


                    }
                }


                if (_dm.Ps.GetAllPlayers().Any())
                {
                    var players = _dm.Ps.GetAllPlayers().ToList();
                    foreach (var p in players)
                    {
                        if ((DateTime.Now - p.date).TotalSeconds > Parameters.deletePlaeyrTimeout*60)
                        {
                            _dm.Ps.DeletePlayer(p);
                        }

                    }







                    Thread.Sleep(10000);

                }




            }



        }
    }
}
