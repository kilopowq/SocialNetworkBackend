using AutoMapper;
using DAL.Models;
using EFModels;

namespace DAL
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserDto>().ReverseMap();
                cfg.CreateMap<Message, MessageDto>().ReverseMap();
                cfg.CreateMap<Conference, ConferenceDto>().ReverseMap();
                cfg.CreateMap<Friend, FriendDto>().ReverseMap();
            });
        }
    }
}
