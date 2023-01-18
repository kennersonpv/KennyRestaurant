﻿using Kenny.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.OrderAPI.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
	}
}
