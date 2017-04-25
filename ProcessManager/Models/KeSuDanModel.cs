using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.ProcessInterface;

namespace ProcessManager.Models
{
    public class KeSuDanModel
    {
        public string khbh { get; set; }
        public string tbrq { get; set; }
        public List<CanPinModel> canpin { get; set; }
        public string fangshi { get; set; }
        public int bid { get; set; }
    }
}