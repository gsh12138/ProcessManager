using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class BiaoListModel
    {
        public string name { get; set; }
        public string detil { get; set; }
        public int pid { get; set; }
        public string url { get; set; }
        
    }

    public class ListBiaoListModel
    {
        public List<BiaoListModel> list { get; set; }
    }
}