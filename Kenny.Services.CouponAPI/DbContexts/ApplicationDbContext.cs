﻿using Kenny.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.CouponAPI.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Coupon> Coupons { get; set; }
	}
}
