using System;
using System.ComponentModel;
using System.Threading;

namespace Rhyous.CS6210.Hw1.Models
{
    public class TimeSimulator
    {
        public void Start(DateTime start, int timeMultiplier, int duration, Action<DateTime> action)
        {
            if (_Started)
                return;
            _Started = true;
            StartDate = start;
            Current = start;
            TimeMultiplier = timeMultiplier;
            EndDate = start.AddDays(duration);
            Action = action;
            Worker = new BackgroundWorker() { WorkerReportsProgress = true };
            Worker.DoWork += CountTime;
            Worker.ProgressChanged += UpdateTime;
            Worker.RunWorkerAsync();
        } private bool _Started = false;

        public BackgroundWorker Worker { get; set; }

        public bool IsReportingProgress { get; set; }

        internal void UpdateTime(object sender, ProgressChangedEventArgs e)
        {
            Current = Current.AddSeconds(TimeMultiplier);
            Action?.Invoke(Current);
            IsReportingProgress = false;
        }

        internal void CountTime(object sender, DoWorkEventArgs e)
        {
            while (!_Shutdown && Current < EndDate)
            {
                Thread.Sleep(1000);
                if (!IsReportingProgress)
                {
                    IsReportingProgress = true;
                    Worker.ReportProgress(1);
                }
            }
        } private bool _Shutdown = false;
        
        public void Shutdown() { _Shutdown = true; }

        /// <summary>
        /// The number of seconds a second represents.
        /// Common times:
        ///      3600 = 1 hour
        ///     86400 = 1 day
        /// </summary>
        public int TimeMultiplier { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Current { get; set; }
        public DateTime EndDate { get; set; }
        public Action<DateTime> Action { get; set; }

        public void Wait()
        {
            while (!_Shutdown && Current < EndDate)
            {
            }
        }
    }
}
