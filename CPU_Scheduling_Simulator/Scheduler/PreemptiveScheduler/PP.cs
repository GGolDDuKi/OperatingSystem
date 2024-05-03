using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class PP : Scheduler
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

            if (ReadyProcess.Count > 0)
            {
                WorkingProcess = FindProcess();
                WorkingProcess.ServiceTime--;
            }
            else
                WorkingProcess = null;
        }
        else if(CheckPriority())
        {
            ReadyProcess.Add(WorkingProcess);
            WorkingProcess = FindProcess();
            WorkingProcess.ServiceTime--;
        }
        else
            WorkingProcess.ServiceTime--;
    }

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
    
    //현재 작업 중인 프로세스보다 우선순위가 높은 프로세스가 대기 중일 경우 true, 아니면 false
    bool CheckPriority()
    {
        if (WorkingProcess == null)
            throw new InvalidOperationException();

        if (ReadyProcess.Count == 0)
            return false;

        Process targetProcess = WorkingProcess;

        foreach (var process in ReadyProcess)
        {
            if (targetProcess.Priority > process.Priority)
                return true;
        }

        return false;
    }
}
