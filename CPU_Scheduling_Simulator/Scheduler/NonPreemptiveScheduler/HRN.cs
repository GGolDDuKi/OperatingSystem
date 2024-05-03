using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class HRN : NPScheduler
{
    public override Process FindProcess()
    {
        Process targetProcess = ReadyProcess[0];

        foreach (var process in ReadyProcess)
        {
            SetPriority(process);
            if (targetProcess.Priority < process.Priority)
            {
                targetProcess = process;
            }
        }

        ReadyProcess.Remove(targetProcess);

        return targetProcess;
    }

    public void SetPriority(Process process)
    {
        process.Priority = (process.Performance.WaitingTime + process.ServiceTime) / (float)process.ServiceTime;
    }
}
