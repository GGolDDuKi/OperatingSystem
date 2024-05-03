using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class RR : PScheduler
{
    public override Process FindProcess()
    {
        Process targetProcess = ReadyProcess[0];
        ReadyProcess.Remove(targetProcess);
        return targetProcess;
    }
}
