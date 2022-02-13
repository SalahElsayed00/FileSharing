using AutoMapper;

namespace FileSharing
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<Data.ApplicationUser, Areas.Admin.Models.AdminUserViewModel>()
                .ForMember(e => e.UserId, options => options.MapFrom(e => e.Id));
        }
    }

    public class ContactProfiles : Profile
    {
        public ContactProfiles()
        {
            CreateMap<Data.Contact, Areas.Admin.Models.ContactUsViewModel>();
                
        }
    }
}
