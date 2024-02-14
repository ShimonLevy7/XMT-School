using System.Timers;

using Timer = System.Timers.Timer;

namespace XmtSchoolWebApi.Events
{
	public class MinuteEvent
	{
		// Declare the event as nullable.
		public event ElapsedEventHandler? Elapsed;

		private readonly Timer timer;

		public MinuteEvent()
		{
			timer = new Timer(60000);
			timer.Elapsed += OnTimerElapsed;
			timer.AutoReset = true;
			timer.Start();
		}

		private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
		{
			// Timer elapsed, invoke.
			Elapsed?.Invoke(sender, e);
		}

		public void Stop()
		{
			timer.Stop();
		}

		public void Dispose()
		{
			timer.Dispose();
		}
	}
}
