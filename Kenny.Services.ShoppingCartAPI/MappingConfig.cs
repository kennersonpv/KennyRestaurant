using AutoMapper;

namespace Kenny.Services.ShoppingCartAPI
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
