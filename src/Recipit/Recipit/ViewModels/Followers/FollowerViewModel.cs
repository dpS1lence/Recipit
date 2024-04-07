namespace Recipit.ViewModels.Followers
{
    using AutoMapper;
    using Recipit.Infrastructure.Data.Models;
    using Recipit.Infrastructure.Mapping;

    public class FollowerViewModel : IMapFrom<Comment>
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Username { get; set; } = default!;

        public void Mapping(Profile map)
        {
            map.CreateMap<FollowerViewModel, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo)).ReverseMap();
        }
    }
}
