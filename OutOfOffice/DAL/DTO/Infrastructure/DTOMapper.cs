using AutoMapper;

namespace OutOfOffice.DAL.DTO.Infrastructure
{
    public static class DTOMapper
    {
        public static TDTO MapToDTO<T, TDTO>(T target)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, TDTO>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<TDTO>(target);
        }

        public static T MapFromDTO<TDTO, T>(TDTO dto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TDTO, T>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<T>(dto);
        }
    }
}
