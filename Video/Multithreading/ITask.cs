using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameApp.Video.Multithreading
{
    public interface ITask
    {
        TimeSpan Time { get; }

        void Process();
    }
}
