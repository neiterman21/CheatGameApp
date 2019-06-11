using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CheatGameApp
{
    public static class TimeStamper
    {
        private static DateTime _baseTimeStamp = DateTime.Now;
        public static DateTime BaseDateTime { get { return _baseTimeStamp; } }

        private static Stopwatch _stopWatch = new Stopwatch();
        public static void Start() { _stopWatch.Start(); }
        public static void Stop() { _stopWatch.Stop(); }
        public static TimeSpan Time
        {
            get
            {
                return
                             _baseTimeStamp.TimeOfDay +
                             _stopWatch.Elapsed;
            }
        }
        public static TimeSpan GetTime() { return Time; }
    }

    public class DurationMeasureLog
    {
        private static List<DurationMeasureLog> _logs = new List<DurationMeasureLog>();
        private List<TimeSpan> _measures = new List<TimeSpan>();

        private TimeSpan _startTime;
        private string _name;

        public DurationMeasureLog(string Name)
        {
            _name = Name;
            _logs.Add(this);
            _startTime = TimeStamper.Time;
        }

        public TimeSpan End()
        {
            var duration = TimeStamper.Time - _startTime;
            _measures.Add(duration);
            return duration;
        }

        public static void Save()
        {
            foreach (var log in _logs)
            {
                string fileName = log._name + "_measure.txt";
                log._measures.ForEach(measure => File.AppendAllText(fileName,
                                                     measure.TotalMilliseconds.ToString() + Environment.NewLine));
            }
        }
    }
        
}
