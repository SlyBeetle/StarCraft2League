using StarCraft2League.Services.Interfaces;
using System;
using System.Timers;

namespace StarCraft2League.Services
{
    public class AlarmClock : IAlarmClock
    {
        private Timer _timer = new Timer();
        private readonly object _objectLock = new Object();

        public AlarmClock()
        {
            _timer.AutoReset = false;
        }

        public event ElapsedEventHandler Rang
        {
            add
            {
                lock(_objectLock)
                    _timer.Elapsed += value;
            }
            remove
            {
                lock(_objectLock)
                    _timer.Elapsed -= value;
            }
        }

        public void Set(DateTime dateTime)
        {
            TimeSpan interval = dateTime - DateTime.Now;
            _timer.Interval = interval.TotalMilliseconds;
            _timer.Start();
        }
    }
}
