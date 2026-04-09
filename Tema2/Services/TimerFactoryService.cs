using System;
using System.Collections.Generic;
using System.Text;

namespace Tema2.Services
{
    internal class TimerFactoryService : Interfaces.ITimerFactory
    {
        public Interfaces.ITimer CreateTimer() => new TimerService();
    }
}
