using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Define
{
    public class Process
    {
        public string ProcessId { get; set; }
        public int ArrivalTime { get; set; }
        public int ServiceTime { get; set; }
        public float Priority { get; set; }
        public int TimeQuantum { get; set; }
        public int UseTime { get; set; }

        public Performance Performance { get; set; }
        public bool Responsed { get; set; }

        public Process(string processId, string arrivalTime, string serviceTime, string priority, string timeQuantum)
        {
            ProcessId = processId;
            ArrivalTime = int.Parse(arrivalTime);
            ServiceTime = int.Parse(serviceTime);
            Priority = float.Parse(priority);
            TimeQuantum = int.Parse(timeQuantum);
            UseTime = 0;
            Performance = new Performance();
            Responsed = false;
        }

        public Process(string processId, int arrivalTime, int serviceTime, float priority, int timeQuantum)
        {
            ProcessId = processId;
            ArrivalTime = arrivalTime;
            ServiceTime = serviceTime;
            Priority = priority;
            TimeQuantum = timeQuantum;
            UseTime = 0;
            Performance = new Performance();
            Responsed = false;
        }
    }

    public class Performance
    {
        public float WaitingTime { get; set; }
        public float ResponseTime { get; set; }
        public float ReturnTime { get; set; }
    }
}
