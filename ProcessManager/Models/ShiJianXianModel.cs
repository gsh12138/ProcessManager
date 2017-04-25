using System;
using System.Collections.Generic;
using System.Linq;
using ProcessBasice.ChangLiang;

namespace ProcessManager.Models
{
    public class ShiJianXianModel
    {
        public ProcessState state { get; set; }
        public string caozuoren { get; set; }
        public string caozuotime { get; set; }

        public string pizhu { get; set; }
    }

    public class ListShiJianXian
    {
        public ListShiJianXian(List<ShiJianXianModel> shijianxian) {
            this.shijianxians = shijianxian;
        }
        public List<ShiJianXianModel> shijianxians { get; set; }
    }


    
}