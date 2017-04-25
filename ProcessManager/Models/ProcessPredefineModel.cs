using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessBasice.Model;

namespace ProcessManager.Models
{
    /// <summary>
    /// 当前步骤表
    /// </summary>
    public class ProcessPredefineModel : PredefineModel, IBuild<ProcessPredefineModel>
    {
        ProcessPredefineModel IBuild<ProcessPredefineModel>.build()
        {
            return new ProcessPredefineModel();
        }

        
    }
}