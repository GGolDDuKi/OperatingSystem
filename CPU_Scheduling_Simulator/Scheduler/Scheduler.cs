using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public abstract class Scheduler
{
    protected Process? WorkingProcess { get; set; }
    protected List<Process> ReadyProcess { get; set; }
    protected List<Process> EndProcess { get; set; }
    protected string GanttChart { get; set; }
    public int Timer { get; set; }

    public Scheduler()
    {
        ReadyProcess = new List<Process>();
        EndProcess = new List<Process>();
        GanttChart = "";
        Timer = 0;
    }

    public abstract void AllocatingCPU();
    public abstract Process FindProcess();

    public void Scheduling()
    {
        do
        {
            AddProcess();
            AllocatingCPU();
            Timer++;

            if (WorkingProcess == null)
                break;

            IncreasingWaitingTime();
            IncreasingResponseTime();
            IncreasingReturnTime();
            SetGanttChart();
        } while (ReadyProcess.Count > 0 || WorkingProcess != null);

        OutputResult();
    }

    public void OutputResult()
    {
        if(ReadyProcess.Count > 0)
        {
            Console.WriteLine("스케줄링 중 오류가 발생하였습니다.");
            return;
        }

        Console.WriteLine("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
        Console.WriteLine(this.GetType().Name + '\n');
        Console.WriteLine(GanttChart + '\n');

        float totalWaitingTime = 0;
        foreach(var process in EndProcess)
        {
            Console.WriteLine($"{process.ProcessId} 대기시간 : {process.Performance.WaitingTime}");
            totalWaitingTime += process.Performance.WaitingTime;
        }
        Console.WriteLine($"평균 대기시간 : {totalWaitingTime / EndProcess.Count}\n");

        float totalResponseTime = 0;
        foreach (var process in EndProcess)
        {
            Console.WriteLine($"{process.ProcessId} 응답시간 : {process.Performance.ResponseTime}");
            totalResponseTime += process.Performance.ResponseTime;
        }
        Console.WriteLine($"평균 응답시간 : {totalResponseTime / EndProcess.Count}\n");

        float totalReturnTime = 0;
        foreach (var process in EndProcess)
        {
            Console.WriteLine($"{process.ProcessId} 반환시간 : {process.Performance.ReturnTime}");
            totalReturnTime += process.Performance.ReturnTime;
        }
        Console.WriteLine($"평균 반환시간 : {totalReturnTime / EndProcess.Count}");
        Console.WriteLine("ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
    }
    
    void AddProcess()
    {
        foreach (var process in Program._processes)
        {
            if (process.ArrivalTime == Timer)
            {
                ReadyProcess.Add(CopyProcess(process));
            }
        }
    }

    void SetGanttChart()
    {
        if (WorkingProcess == null)
            GanttChart += "|ㅡ|";
        else
            GanttChart += $"|{WorkingProcess.ProcessId}|";
    }

    void IncreasingWaitingTime()
    {
        foreach(var process in ReadyProcess)
        {
            if (process.ArrivalTime >= Timer)
                continue;

            process.Performance.WaitingTime++;
        }
    }

    void IncreasingResponseTime()
    {
        if (WorkingProcess != null)
            if (WorkingProcess.Responsed == false)
                WorkingProcess.Performance.ResponseTime++;

        foreach (var process in ReadyProcess)
        {
            if (process.ArrivalTime >= Timer)
                continue;

            if (process.Responsed == false)
                process.Performance.ResponseTime++;
        }
    }

    void IncreasingReturnTime()
    {
        if(WorkingProcess != null)
            WorkingProcess.Performance.ReturnTime++;

        foreach (var process in ReadyProcess)
        {
            if (process.ArrivalTime >= Timer)
                continue;

            process.Performance.ReturnTime++;
        }
    }

    public Process CopyProcess(Process process)
    {
        Process addProcess = new Process(process.ProcessId, process.ArrivalTime, process.ServiceTime, process.Priority, process.TimeQuantum);
        return addProcess;
    }
}
