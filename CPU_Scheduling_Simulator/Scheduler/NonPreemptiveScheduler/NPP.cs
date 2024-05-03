using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class NPP : NPScheduler
{
    public override Process FindProcess()
    {
        Process targetProcess = ReadyProcess[0];

        foreach (var process in ReadyProcess)
        {
            if (targetProcess.Priority > process.Priority)
            {
                targetProcess = process;
            }
        }

        ReadyProcess.Remove(targetProcess);

        return targetProcess;
    }
}
