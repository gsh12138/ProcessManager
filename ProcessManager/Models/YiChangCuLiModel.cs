using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class YiChangCuLiModel
    {
        public YiChang yichang{ get; set; }
        public List<CuLiGuoCheng> chuliguocheng { get; set; }
    }

    public class YiChang {
        public string id { get; set; }
        public string tbrq { get; set; }
        public string zt { get; set; }
        public string cxrq { get; set; }
        public string tbbm { get; set; }
        public string tbr { get; set; }
        public string ycfl { get; set; }
        public string ycms { get; set; }
        public YiChangState state { get; set; }
    }

    public class CuLiGuoCheng
    {
        public CuLiGuoCheng() { }
        public CuLiGuoCheng(string id) {
            this.id = id;
        }
        public string id { get; set; }
        public string yyfx { get; set; }
        public string zgjg { get; set; }
        public string xgqr { get; set; }
        public string culitime { get; set; }
        public string leixing { get; set; }
        public string culiren { get; set; }
        public string flag { get; set; }

        public int jishu { get; set; }
    }
    public enum CuLiLeiXing
    {
        处理异常,反馈异常
    }

    public enum YiChangState
    {
        未处理=1,
        已处理=2,
        未解决=3,
        待验证=4,
        已关闭=5
    }
}