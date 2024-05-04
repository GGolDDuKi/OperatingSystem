using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public abstract class Scheduler
{
    #region 스케줄링 및 출력에 필요한 필드
    protected Process? WorkingProcess { get; set; }
    protected List<Process> ReadyProcess { get; set; }
    protected List<Process> EndProcess { get; set; }
    protected string GanttChart { get; set; }
    public int Timer { get; set; }
    #endregion

    public Scheduler()
    {
        ReadyProcess = new List<Process>();
        EndProcess = new List<Process>();
        GanttChart = "";
        Timer = 0;
    }

    //모든 스케줄링 알고리즘들은 해당 메서드들을 오버라이드하여 CPU를 할당받을 프로세스를 찾고, CPU를 할당
    public abstract void AllocatingCPU();
    public abstract Process FindProcess();

    //스케줄링을 실행
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

    //스케줄링을 수행한 결과를 출력
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

    //작업이 완료된 프로세스 처리
    protected void ProcessEnd()
    {
        if (WorkingProcess == null)
            return;

        ProcessResponse();
        EndProcess.Add(WorkingProcess);
        ChangeWorkingProcess();
    }

    //WorkingProcess에 할당된 프로세스가 없을 경우 찾아서 할당
    protected void NoWorkingProcess()
    {
        if (WorkingProcess == null)
            WorkingProcess = FindProcess();
    }

    //타임슬라이스를 모두 사용한 프로세스 처리
    protected void EndTimeSlice()
    {
        if (WorkingProcess == null)
            return;

        ProcessResponse();
        ReadyProcess.Add(WorkingProcess);
        ChangeWorkingProcess();
    }

    //CPU할당할 프로세스를 교환하고 작업 수행
    protected void ChangeWorkingProcess()
    {
        if (ReadyProcess.Count > 0)
        {
            WorkingProcess = FindProcess();
            Work();
        }
        else
            WorkingProcess = null;
    }

    //CPU가 할당된 프로세스의 작업을 수행
    protected void Work()
    {
        if (WorkingProcess == null)
            return;

        WorkingProcess.ServiceTime--;
        WorkingProcess.UseTime++;
    }

    //프로세스의 응답 여부
    protected void ProcessResponse()
    {
        if (WorkingProcess == null)
            return;

        if (WorkingProcess.Responsed == false)
            WorkingProcess.Responsed = true;

        WorkingProcess.UseTime = 0;
    }

    //프로세스의 도착시간에 맞춰 준비리스트에 프로세스를 추가
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

    //현재 CPU가 할당된 프로세스를 간트차트에 표시
    void SetGanttChart()
    {
        if (WorkingProcess == null)
            GanttChart += "|ㅡ|";
        else
            GanttChart += $"|{WorkingProcess.ProcessId}|";
    }

    //준비상태인 프로세스의 대기시간 증가
    void IncreasingWaitingTime()
    {
        foreach(var process in ReadyProcess)
        {
            process.Performance.WaitingTime++;
        }
    }

    //현재까지 응답하지 않은 프로세스의 응답시간 증가
    void IncreasingResponseTime()
    {
        if (WorkingProcess != null)
            if (WorkingProcess.Responsed == false)
                WorkingProcess.Performance.ResponseTime++;

        foreach (var process in ReadyProcess)
        {
            if (process.Responsed == false)
                process.Performance.ResponseTime++;
        }
    }

    //완료되지 않은 프로세스의 반환시간 증가
    void IncreasingReturnTime()
    {
        if(WorkingProcess != null)
            WorkingProcess.Performance.ReturnTime++;

        foreach (var process in ReadyProcess)
        {
            process.Performance.ReturnTime++;
        }
    }

    //프로세스를 준비리스트에 추가할 때, 기존 프로세스 정보를 깊은 복사하여 반환
    public Process CopyProcess(Process process)
    {
        Process addProcess = new Process(process.ProcessId, process.ArrivalTime, process.ServiceTime, process.Priority, process.TimeQuantum);
        return addProcess;
    }
}
