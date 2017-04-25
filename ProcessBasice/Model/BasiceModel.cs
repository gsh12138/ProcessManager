using System;

namespace ProcessBasice.Model
{

    public class BasiceModel: IComparable
    {
        private int _pid;
        private int _bid;
        private int _order;
        private string _handler;
        private string _nexthandler;
        private string _lasthandler;
        private int _state;

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

        public int Bid
        {
            get
            {
                return _bid;
            }

            set
            {
                _bid = value;
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

        public string Handler
        {
            get
            {
                return _handler;
            }

            set
            {
                _handler = value;
            }
        }

        public string Nexthandler
        {
            get
            {
                return _nexthandler;
            }

            set
            {
                _nexthandler = value;
            }
        }

        public string Lasthandler
        {
            get
            {
                return _lasthandler;
            }

            set
            {
                _lasthandler = value;
            }
        }

        public int State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        int IComparable.CompareTo(object obj)
        {
            BasiceModel other = (BasiceModel)obj;
            return this.Order - other.Order;
        }
    }
}
