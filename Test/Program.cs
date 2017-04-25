using System.Collections.Generic;
using ProcessManager.Models;


namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ProcessProcessModel> processmodels = ProcessCreatHelper.createProcess(1, 1, new List<string>()
            {
                "高尚1","高尚2","高尚3","高尚4","高尚5","高尚6","高尚7","高尚8","高尚9"
            });

            ProcessProcessDAO dao = new ProcessProcessDAO();
            dao.insertProcessModel(processmodels[0]);
        }
    }
}
