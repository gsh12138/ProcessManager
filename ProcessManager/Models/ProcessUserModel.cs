using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProcessManager.Models
{
    public class ProcessUserModel
    {
        [Required(ErrorMessage ="必填")]
        [Display(Name ="用户名")]
        [DataType(DataType.Text)]
        public string user { get; set; }


        [Required]
        [Display(Name ="密码")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name ="记住我？")]
        public bool RememberMe { get; set; }

        public string xingming { get; set; }
    }
}