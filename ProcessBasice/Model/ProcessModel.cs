using ProcessBasice.ChangLiang;
using System;
namespace ProcessBasice.Model
{
    public class ProcessModel:BasiceModel
    {
        private ProcessState _state;
        private DateTime _sdate;
        public new ProcessState State
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

        public DateTime sdate {
            get { return _sdate; }
            set { _sdate = value; }
        }
        

        public override string ToString()
        {
            return "pid:" + Pid + ";bid:" + Bid + ";+order:" + Order + ";hanlder:" + Handler + ";nexthanlder:" + Nexthandler +
                ";lasthanlder:" + Lasthandler + ";state:" + State;
        }
    }
}
