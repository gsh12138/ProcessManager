using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.Models;

namespace ProcessManager.Models
{
    public class ProcessListProcessModel
    {
        private List<ProcessProcessModel> _listprocessmodel;

        public List<ProcessProcessModel> Listprocessmodel
        {
            get
            {
                return _listprocessmodel;
            }

            set
            {
                _listprocessmodel = value;
            }
        }
    }
}