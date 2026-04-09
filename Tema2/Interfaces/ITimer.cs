using System;
using System.Collections.Generic;
using System.Text;

namespace Tema2.Interfaces
{
    public interface ITimer
    {
        TimeSpan Interval { get; set; }
        bool IsEnabled { get; set; }
        void Start();
        void Stop();
        event EventHandler Tick;
    }
}
