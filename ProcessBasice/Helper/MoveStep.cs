using ProcessBasice.Model;
using ProcessBasice.ChangLiang;
using System.Collections.Generic;

namespace ProcessBasice.Helper
{
    public class MoveStep<T> where T:ProcessModel
    {
        private List<T> templprocess=new List<T>();
        private T tempprocess;
        public  void downStep(PredefineModel predefine,List<T> lprocess)
        {
            templprocess.Clear();
            lprocess.ForEach(i => templprocess.Add(i));
            
            foreach (T process in templprocess)
            {
                if (process.Order==predefine.Order)
                {
                    if (predefine.Nexthandler.Equals(PredefineState.FINISH.ToString()))
                    {
                        predefine.State = PredefineState.FINISH;
                        process.State = ProcessState.FINISH;
                        return;
                    }
                    T nextprocess = findNextStep(process, lprocess, Move.DOWN);
                    lprocess[lprocess.IndexOf(process)].State = ProcessState.FINISH;
                    lprocess[lprocess.IndexOf(process) + 1].State = ProcessState.DEFINE;
                    processToPredefine(nextprocess, predefine);
                    predefine.State = PredefineState.PROCESSING;
                    break;    
                }
            }
            
            
        }

        public void upStep(PredefineModel predefine,List<T> lprocess)
        {
            templprocess.Clear();
            lprocess.ForEach(i => templprocess.Add(i));
            foreach (T process in lprocess)
            {
                if (process.Order == predefine.Order)
                {
                    T nextprocess = findNextStep(process, lprocess, Move.UP);
                    lprocess[lprocess.IndexOf(process)].State = ProcessState.BACK;
                    lprocess[lprocess.IndexOf(process) - 1].State = ProcessState.DEFINE;
                    processToPredefine(nextprocess, predefine);
                    predefine.State = PredefineState.BACK;
                    break;
                }
            }
        }

        public void returnStep(PredefineModel predefine,List<T> lprocess)
        {
            templprocess.Clear();
            lprocess.ForEach(i => templprocess.Add(i));
            foreach (T process in lprocess)
            {
                if (process.Order == predefine.Order)
                {
                    T nextprocess = findNextStep(process, lprocess, Move.BACK);
                    lprocess[lprocess.IndexOf(process)].State = ProcessState.RETURN;
                    lprocess[0].State = ProcessState.RETURN;
                    processToPredefine(nextprocess, predefine);
                    predefine.State = PredefineState.RETURN;
                    break;
                }
            }
        }

        public void insertStep(PredefineModel predefine,List<T> lprocess,T process)
        {
            templprocess.Clear();
            lprocess.ForEach(i => templprocess.Add(i));
            foreach (T p in templprocess)
            {
                if (p.Order == predefine.Order)
                {
                    lprocess[templprocess.IndexOf(p)].State = ProcessState.JIAQIAN;
                    lprocess.Add(process);
                    break;
                }
            }
            processToPredefine(process, predefine);
            predefine.State = PredefineState.JIAQIAN;
        }
        

        

        private T findNextStep(T process,IList<T> lprocess,Move m)
        {
            tempprocess = null;
            List<T> listprocess = (List<T>)lprocess;
            listprocess.Sort();
            switch (m)
            {
                case Move.UP:
                    tempprocess = listprocess[listprocess.IndexOf(process) - 1];
                    break;
                case Move.DOWN:
                    tempprocess = listprocess[listprocess.IndexOf(process) + 1];
                    break;
                case Move.BACK:
                    tempprocess = listprocess[0];
                    break;
                default:
                    break;
            }
            return tempprocess;
        }

        private void processToPredefine(T process,PredefineModel predefine) {
            predefine.Order = process.Order;
            predefine.Handler = process.Handler;
            predefine.Nexthandler = process.Nexthandler;
            predefine.Lasthandler = process.Lasthandler;
        }


    }

    public class CheckState
    {
        public bool isHuiQianFinish(List<HuiQian> lhuiqian)
        {
            foreach (HuiQian huiqian in lhuiqian)
            {
                if (huiqian.State == HuiQianState.DEFINE)
                {
                    return false;
                }
            }
            return true;
        }

        public void changeChaoSongState(List<ChaoSong> lchaosong)
        {
            lchaosong.ForEach(chaosong =>
            {
                chaosong.State = ChaoSongState.FINISH;
            });
        }
    }

    
}
