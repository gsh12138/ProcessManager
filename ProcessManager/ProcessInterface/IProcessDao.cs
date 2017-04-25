using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessBasice.ChangLiang;

namespace ProcessManager.ProcessInterface
{
    interface IProcessDao<M>
    {
        List<M> selectById(int id);
        int update(M model);
        int insert(M model);
        int insertList(List<M> lModel);

    }
}
