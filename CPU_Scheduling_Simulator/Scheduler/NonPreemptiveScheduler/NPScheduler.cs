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
        NoWorkingProcess();

        if (WorkingProcess == null)
            return;

        if (WorkingProcess.ServiceTime == 0)
        {
            ProcessEnd();
        }
        else
            Work();
    }

    public abstract override Process FindProcess();
}
