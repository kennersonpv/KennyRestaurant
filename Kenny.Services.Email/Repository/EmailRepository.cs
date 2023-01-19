using Kenny.Services.Email.DbContexts;
using Kenny.Services.Email.Messages;
using Kenny.Services.Email.Models;
using Kenny.Services.Email.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.Email.Repository
{
	public class EmailRepository : IEmailRepository
	{
		private readonly DbContextOptions<ApplicationDbContext> _dbContext;
		public EmailRepository(DbContextOptions<ApplicationDbContext> dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SendAndLogEmailAsync(UpdatePaymentResultMessage message)
		{
			//implement email sender
			var emailLog = new EmailLog()
			{
				Email = message.Email,
				EmailSent = DateTime.Now,
				Log = $"Order - {message.OrderId} has been created"
			};

			await using var _db = new ApplicationDbContext(_dbContext);
			_db.EmailLogs.Add(emailLog);
			await _db.SaveChangesAsync();
		}
	}
}
