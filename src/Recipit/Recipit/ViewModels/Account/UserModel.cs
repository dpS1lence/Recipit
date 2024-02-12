using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;

namespace Recipit.ViewModels.Account
{
    public class UserModel : IMapFrom<RecipitUser>
    {
        public string Id { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public string UserName { get; set; } = default!;

        public void Mapping(Profile map)
        {
            map.CreateMap<RecipitUser,  UserModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
        }
    }
}
