﻿using Kenny.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.Email.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<EmailLog> EmailLogs { get; set; }
	}
}
