using System;
using System.Collections.Generic;
using System.Text;
using DL.Interfaces;
using DL.Implementations;
namespace DL
{
    class DataManager
    {
        IPlayerSrv _ps;
        IRoomSrv _rs;

        public DataManager()
        {
            DataStor ds = new DataStor();
            _ps = new PlaerSrv(ds);
            _rs = new RoomSrv(ds);
        }
        public IPlayerSrv Ps { get { return _ps; } }
        public IRoomSrv Rs { get { return _rs; } }
    }
}
