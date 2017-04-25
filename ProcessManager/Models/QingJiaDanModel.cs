using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProcessManager.ProcessInterface;

namespace ProcessManager.Models
{
    public class QingJiaDanModel
    {
        [Display(Name ="表单号")]
        public int bid { get; set; }

        [Display(Name = "请假类别")]
        [Required]
        [DataType(DataType.Text)]
        public QingJiaLeiXing leixing { get; set; }

        [Required(ErrorMessage = "开始时间未填写")]
        [Display(Name ="开始休假时间")]
        
        public string startime { get; set; }

        [Required(ErrorMessage = "终止时间未填写")]
        [Display(Name ="终止休假时间")]
        
        public string finishtime { get; set; }

        [Display(Name ="休假天数")]
        public int tianshu { get; set; }

        [Required(ErrorMessage ="请假缘由未填写")]
        [Display(Name ="详细信息")]
        public string detil { get; set; }

        public string fangshi { get; set; }

        public string shenqingren { get; set; }

        public BiaoDanHeadModel head { get; set; }
    }

    public enum QingJiaLeiXing
    {
        病假=1,
        事假=2,
        婚假=3,
        其他=4
    }
}