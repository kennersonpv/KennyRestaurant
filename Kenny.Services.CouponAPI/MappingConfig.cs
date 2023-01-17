using AutoMapper;
using Kenny.Services.CouponAPI.Models;
using Kenny.Services.CouponAPI.Models.Dto;

namespace Kenny.Services.CouponAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<CouponDto, Coupon>().ReverseMap();
			});

			return mappingConfig;
		}
	}
}
