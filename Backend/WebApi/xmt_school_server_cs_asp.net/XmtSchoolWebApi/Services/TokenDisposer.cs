using XmtSchoolDatabase;

using XmtSchoolTypes.Login;

using XmtSchoolWebApi.Events;

namespace XmtSchoolWebApi.Services
{
	public class TokenDisposer : IHostedService, IDisposable
	{
		public byte TokenMaxLifeInMinutes = 30; // Max life in minutes.

		private readonly IServiceScopeFactory _scopeFactory;
		private readonly MinuteEvent _minuteEvent = new MinuteEvent();

		public TokenDisposer(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Add minute event.
			_minuteEvent.Elapsed += TokenDispose_MinuteEvent;

			return Task.CompletedTask;
		}

		private void TokenDispose_MinuteEvent(object? sender, System.Timers.ElapsedEventArgs e)
		{
			using (IServiceScope scope = _scopeFactory.CreateScope())
			{
				XmtSchoolDbContext scopedDbContext = scope.ServiceProvider.GetRequiredService<XmtSchoolDbContext>();

				// Filter tokens based on age, find expired ones.
				List<Token> tokensToDispose = scopedDbContext.Tokens
					.ToList() // Materialize the data in memory as we can't translate the filter to SQL.
					.Where(t => (DateTime.UtcNow - t.LastUsed).TotalMinutes > TokenMaxLifeInMinutes)
					.ToList();

				// Remove them as range.
				scopedDbContext.RemoveRange(tokensToDispose);
				scopedDbContext.SaveChanges();
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			// Clean up any resources here if necessary.
			_minuteEvent.Stop();

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_minuteEvent.Dispose();

			GC.SuppressFinalize(this);
		}
	}
}
