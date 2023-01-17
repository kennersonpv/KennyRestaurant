using AutoMapper;
using Kenny.Services.CouponAPI.DbContexts;
using Kenny.Services.CouponAPI.Models.Dto;
using Kenny.Services.CouponAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.CouponAPI.Repository
{
	public class CouponRepository : ICouponRepository
	{
		private readonly ApplicationDbContext _db;
		private IMapper _mapper;

		public CouponRepository(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<CouponDto> GetCouponByCodeAsync(string couponCode)
		{
			var coupon = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
			return _mapper.Map<CouponDto>(coupon);
		}
	}
}
