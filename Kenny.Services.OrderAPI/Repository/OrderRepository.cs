﻿using Kenny.Services.OrderAPI.DbContexts;
using Kenny.Services.OrderAPI.Models;
using Kenny.Services.OrderAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
	{
		private readonly DbContextOptions<ApplicationDbContext> _dbContext;
		public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> AddOrderAsync(OrderHeader orderHeader)
		{
			try
			{
				await using var _db = new ApplicationDbContext(_dbContext);
				_db.OrderHeaders.Add(orderHeader);
				await _db.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
		{
			await using var _db = new ApplicationDbContext(_dbContext);
			var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
			if(orderHeaderFromDb != null)
			{
				orderHeaderFromDb.PaymentStatus = paid;
				await _db.SaveChangesAsync();
			}
		}
	}
}
