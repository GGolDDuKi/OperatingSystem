using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class FCFS : NPScheduler
{
    public override Process FindProcess()
    {
        Process targetProcess = ReadyProcess[0];

        foreach (var process in ReadyProcess)
        {
            if (targetProcess.ArrivalTime > process.ArrivalTime)
            {
                targetProcess = process;
            }
        }

        ReadyProcess.Remove(targetProcess);

        return targetProcess;
    }
}
