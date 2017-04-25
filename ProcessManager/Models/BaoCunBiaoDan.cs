using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class BaoCunBiaoDan
    {
        public int bid { get; set; }
        public string leixing { get; set; }
        public string url { get; set; }
        public List<BaoCunBiaoDan> lbcb { get; set; }
    }
}