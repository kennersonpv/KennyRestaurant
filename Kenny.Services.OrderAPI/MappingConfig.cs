﻿using AutoMapper;

namespace Kenny.Services.OrderAPI
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
			});

			return mappingConfig;
		}
	}
}
