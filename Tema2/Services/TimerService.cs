using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Threading;
using Tema2.Interfaces;

namespace Tema2.Services
{
    internal class TimerService : Interfaces.ITimer
    {
        private readonly DispatcherTimer internalTimer;

        public TimerService()
        {
            internalTimer = new DispatcherTimer();
        }

        public bool IsEnabled
        {
            get => internalTimer.IsEnabled;
            set => internalTimer.IsEnabled = value;
        }

        public TimeSpan Interval
        {
            get => internalTimer.Interval;
            set => internalTimer.Interval = value;
        }

        public void Start() => internalTimer.Start();
        public void Stop() => internalTimer.Stop();

        public event EventHandler Tick
        {
            add => internalTimer.Tick += value;
            remove => internalTimer.Tick -= value;
        }
    }
}
