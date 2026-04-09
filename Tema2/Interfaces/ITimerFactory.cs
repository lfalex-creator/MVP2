using System;
using System.Collections.Generic;
using System.Text;

namespace Tema2.Interfaces
{
    public interface ITimerFactory
    {
        Interfaces.ITimer CreateTimer();
    }
}
