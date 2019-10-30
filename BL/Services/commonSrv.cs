using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Services
{
    public class commonSrv
    {

        public bool getInterval(DateTime d, int sec, char sign)
        {
            TimeSpan ts = DateTime.Now - d;
            if(sign=='>')
            return ts.TotalSeconds > sec;
            else
            if (sign == '<')
                return ts.TotalSeconds < sec;

            return false;
        }

    }
}
