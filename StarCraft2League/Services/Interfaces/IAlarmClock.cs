using System;
using System.Timers;

namespace StarCraft2League.Services.Interfaces
{
    public interface IAlarmClock
    {
        void Set(DateTime dateTime);
        event ElapsedEventHandler Rang;
    }
}
