using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    /// <summary>
    /// 批注表
    /// </summary>
    public class ProcessPiZhu
    {
        private int _pid;
        private int _order;
        private string _hanlder;
        private string _detail;
        private DateTime _pizhutime;

        public int Pid
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

        public int Order
        {
            get
            {
                return _order;
            }

            set
            {
                _order = value;
            }
        }

        public string Hanlder
        {
            get
            {
                return _hanlder;
            }

            set
            {
                _hanlder = value;
            }
        }

        public string Detail
        {
            get
            {
                return _detail;
            }

            set
            {
                _detail = value;
            }
        }

        public DateTime pizhutime {
            get {
                return _pizhutime;
            }
            set {
                _pizhutime = value;
            }
        }
    }
}