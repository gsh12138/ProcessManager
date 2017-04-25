using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.BiaoDan
{
    public interface IHead
    {
        string getFaQiRen();
        DateTime getFaQiTime();

        int getPid();
        
    }
}
