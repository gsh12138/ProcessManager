using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class ProcessInfo:IComparable<ProcessInfo>
    {
        public ProcessPredefineModel predefine
        { get; set; }
        public List<ProcessProcessModel> lprocess
        { get; set; }
        public string processstate { get; set; }
        public string predefinestate { get; set; }
        public string leixing { get; set; }
        public string url { get; set; }

        public int CompareTo(ProcessInfo other)
        {
            return other.predefine.Bid-predefine.Bid;
        }
    }

    public class ProcessListInfo
    {
        public List<ProcessInfo> linfo
        {
            get;set;
        }

        public string userid { get; set; }
        public string userxm { get; set; }
        public string userbm { get; set; }
        
    }
}