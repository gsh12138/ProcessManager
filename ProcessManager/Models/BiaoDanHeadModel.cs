using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class BiaoDanHeadModel {
        public string name { get; set; }

        public string state { get; set; }

        public string faqiren { get; set; }

        public string lastshenhedate { get; set; }

        public string lastshenheren { get; set; }

        public string creatdate { get; set; }

        public List<CanYuRen> canyuren { get; set; }

        public string jindu { get; set; }

        public string jieshao { get; set; }

        public BiaoDanState isShenHE { get; set; }

        public ListShiJianXian shijianxian { get; set; }

        
    }

    public enum BiaoDanState {
        faqi,
        shenhe,
        chakan
    }

    public class CanYuRen{

        public CanYuRen(string name,string state) {
            this.name = name;
            this.state = state;
        }
        public string name { get; set; }
        public string state { get; set; }
    }
}