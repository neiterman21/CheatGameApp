using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CheatGameModel;
using System.IO;

namespace CheatGameApp.Video.Multithreading
{
    public sealed class TaskResults : IEnumerable<TaskResult>
    {
        private readonly List<TaskResult> m_results;
        private bool m_saveWhenCompleted = false;
        //private MoveType m_myMove;
        //private MoveType m_opponentMove;
        private string m_fileName;

        public int TotalTaskCount = 0;
        public int Pending = 0;
        public readonly string Folder;
        public int Count { get { return m_results.Count; } }

        public TaskResults(string folder)
        {
            Pending = 0;
            this.Folder = folder;
            m_results = new List<TaskResult>();
        }
        public void Add(TimeSpan time, string points)
        {
            lock (this)
            {
                TaskResult item = new TaskResult(time, points);
                m_results.Add(item);
                Interlocked.Decrement(ref this.Pending);

                //check if it is last task and needs to be saved
                if (m_saveWhenCompleted && this.Pending == 0)
                    Save();
            }
        }
        //public void BeginSave(MoveType myMove, MoveType opponentMove)
        //{
        //    lock (this)
        //    {
        //        m_myMove = myMove;
        //        m_opponentMove = opponentMove;

        //        m_fileName = Folder + "\\" + string.Format("Points_{0}_{1}", m_myMove, m_opponentMove);
        //        m_saveWhenCompleted = true;
        //    }
        //}
        public void BeginSave(string fileName)
        {
            lock (this)
            {
                m_fileName = fileName;
                m_saveWhenCompleted = true;
            }
        }
        private string GetPoints()
        {
            IEnumerable<string> points = from item in m_results select item.Points;
            string s = string.Join(Environment.NewLine, points);
            return s;
        }
        private void Save()
        {
            m_results.Sort();
            string points = GetPoints();

            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            //write points
            string pointFileName = m_fileName + DateTime.Now.ToString("yyyyMMdd-HH_mm_ss") + ".txt";// Folder + "\\" + string.Format("Points_{0}_{1}_{2:yyyyMMdd-HH_mm_ss}.txt", m_myMove, m_opponentMove, DateTime.Now);
            File.WriteAllText(pointFileName, points);
            m_results.Clear();
            m_results.TrimExcess();
            GC.Collect();
        }

        public IEnumerator<TaskResult> GetEnumerator()
        {
            return m_results.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
