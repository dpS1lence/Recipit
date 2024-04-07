using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using Recipit.Models.Account;

namespace Recipit.Infrastructure.Extensions.Contracts
{
    public class UserSettings : IMapFrom<Comment>
    {
        public string UserName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = default!;
        public string Photo { get; set; } = default!;

        public void Mapping(Profile map)
        {
            map.CreateMap<UserSettings, Comment>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.CreationDate, cfg => cfg.MapFrom(src => DateTime.UtcNow));
        }
    }
}
