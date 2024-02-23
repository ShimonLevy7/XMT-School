using System;
using System.Threading;

namespace XmtSchool_TeachersApp.Utils
{
    public class MtTimer
    {
        public TimeSpan Ticks { get; private set; }
        public uint Count { get; private set; }
        public Action Callback { get; private set; }

        public MtTimer(TimeSpan ticks, uint count, Action callback)
        {
            Ticks = ticks;
            Count = count;
            Callback = callback;
        }

        public Thread? TimerThread { get; private set; } = null;

        public void Start()
        {
            if (TimerThread is not null)
                throw new Exception("Timer has already started.");

            TimerThread = new Thread(() =>
            {
                // Mark this thread as a background thread.
                Thread.CurrentThread.IsBackground = true;

                uint i = Count;

                while (Count == 0 || i > 0)
                {
                    // If not infinite loop.
                    if (Count != 0)
                        i--;

                    Thread.Sleep(Ticks);

                    Callback.Invoke();
                }
            });

            TimerThread.Start();
        }
    }
}
