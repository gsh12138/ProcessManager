using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Helper
{
    public class PidHelper
    {

        private int _pid=0;
        private static PidHelper pidh;

        public int pid
        {
            get
            {
                return _pid;
            }
            set
            {
                _pid = value;
            }

        }

        

        private PidHelper() { }

        public static PidHelper pidFactary()
        {
            if (pidh == null)
            {

                pidh= new PidHelper();
                return pidh;
            }
            return pidh;
        }
    }
}