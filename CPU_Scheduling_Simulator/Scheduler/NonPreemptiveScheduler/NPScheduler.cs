using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public abstract class NPScheduler : Scheduler
{
    public override void AllocatingCPU()
    {
        if (WorkingProcess == null)
        {
            WorkingProcess = FindProcess();
        }

        if (WorkingProcess.ServiceTime == 0)
        {
            WorkingProcess.Responsed = true;
            EndProcess.Add(WorkingProcess);

            if (ReadyProcess.Count > 0)
            {
                WorkingProcess = FindProcess();
                WorkingProcess.ServiceTime--;
            }
            else
                WorkingProcess = null;
        }
        else
            WorkingProcess.ServiceTime--;
    }

    public abstract override Process FindProcess();
}
