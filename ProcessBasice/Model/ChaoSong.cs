using ProcessBasice.ChangLiang;

namespace ProcessBasice.Model
{
    public class ChaoSong
    {
        private int _pid;
        private int _order;
        private string _hanlder;
        private ChaoSongState _state;

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

        public ChaoSongState State
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
    }
}
