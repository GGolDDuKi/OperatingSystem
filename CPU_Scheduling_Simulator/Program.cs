using System;
using System.IO;
using static Define;

public class Program
{
    public static List<Process> _processes = new List<Process>();

    static void Main(string[] args)
    {
        ReadProcessInput(@"..\..\..\ProcessInput.txt");

        FCFS fcfs = new FCFS();
        fcfs.Scheduling();

        SJF sjf = new SJF();
        sjf.Scheduling();

        NPP npp = new NPP();
        npp.Scheduling();

        PP pp = new PP();
        pp.Scheduling();

        RR rr = new RR();
        rr.Scheduling();

        SRT srt = new SRT();
        srt.Scheduling();

        HRN hrn = new HRN();
        hrn.Scheduling();
    }

    static void ReadProcessInput(string filePath)
    {
        string[] processInput = File.ReadAllLines(filePath);

        foreach (string input in processInput)
        {
            string[] parts = input.Split(' ');
            Process process = new Process(parts[0], parts[1], parts[2], parts[3], parts[4]);
            _processes.Add(process);
        }
    }
}