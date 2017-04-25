using System;
using ProcessBasice.ChangLiang;
namespace ProcessBasice.Model
{
   public  class PredefineModel:BasiceModel,IBuild<PredefineModel>
    {
        private PredefineState _state;

        public new PredefineState State
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

        public override string ToString()
        {
            return "pid:" + Pid + ";bid:" + Bid + ";+order:" + Order + 
                ";hanlder:" + Handler + ";nexthanlder:" + Nexthandler +
                ";lasthanlder:" + Lasthandler + ";state:" + State;
        }

        PredefineModel IBuild<PredefineModel>.build()
        {
            return new PredefineModel();
        }
    }
}
