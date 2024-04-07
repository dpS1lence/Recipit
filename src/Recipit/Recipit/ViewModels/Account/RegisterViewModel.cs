using AutoMapper;
using Recipit.Infrastructure.Data.Models;
using Recipit.Infrastructure.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Recipit.Models.Account
{
    public class RegisterViewModel : IMapFrom<RecipitUser>
    {
        public string Email { get; set; } = default!;

        [MinLength(8, ErrorMessage = "Паролата трябва да е минимум 8 символа")]
        public string Password { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        [RegularExpression(@"^[a-zA-Z0-9\-\._]+$", ErrorMessage = "Моля използвайте само латиски букви, цифри и '-', '.', '_'")]
        public string Username { get; set; } = default!;

        public void Mapping(Profile map)
        {
            map.CreateMap<RegisterViewModel, RecipitUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Photo, 
                    cfg => cfg.MapFrom(src => "https://brightspotcdn.byu.edu/dims4/default/8325ccb/2147483647/strip/true/crop/340x340+0+0/resize/1200x1200!/quality/90/?url=https%3A%2F%2Fbrigham-young-brightspot.s3.amazonaws.com%2Fbd%2F7f%2Face2612141aa8c6ad180b0786739%2Fdefault-pfp.jpg"))
                .ForMember(dest => dest.CreationDate, cfg => cfg.MapFrom(src => DateTime.UtcNow));
        }
    }
}
