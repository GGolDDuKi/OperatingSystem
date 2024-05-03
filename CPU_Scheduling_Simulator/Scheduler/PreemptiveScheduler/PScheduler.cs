using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public abstract class PScheduler : Scheduler
{
    public override void AllocatingCPU()
    {
        if (WorkingProcess == null)
        {
            WorkingProcess = FindProcess();
        }

        if (WorkingProcess.ServiceTime == 0)
        {
            if (WorkingProcess.Responsed == false)
                WorkingProcess.Responsed = true;

            EndProcess.Add(WorkingProcess);
            ChangeWorkingProcess();
        }
        else if (WorkingProcess.UseTime >= WorkingProcess.TimeQuantum)
        {
            if (WorkingProcess.Responsed == false)
                WorkingProcess.Responsed = true;

            WorkingProcess.UseTime = 0;
            ReadyProcess.Add(WorkingProcess);
            ChangeWorkingProcess();
        }
        else
        {
            WorkingProcess.ServiceTime--;
            WorkingProcess.UseTime++;
        }
    }

    void ChangeWorkingProcess()
    {
        if (ReadyProcess.Count > 0)
        {
            WorkingProcess = FindProcess();
            WorkingProcess.ServiceTime--;
            WorkingProcess.UseTime++;
        }
        else
            WorkingProcess = null;
    }

    public abstract override Process FindProcess();
}
