using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CheatGameApp.Video.Multithreading
{
    public sealed class TaskResult : IComparable<TaskResult>
    {
        public readonly TimeSpan Time;
        public readonly string Points;

        public TaskResult(TimeSpan time, string points)
        {
            this.Time = time;
            this.Points = points;
        }

        public int CompareTo(TaskResult other)
        {
            return TimeSpan.Compare(this.Time, other.Time);
        }
    }
}
