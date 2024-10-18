using AutoMapper;
using Library.DAL.Models;
using Library.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Library.PL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<Book, BookVM>().ReverseMap();
            CreateMap<ApplicationUser, UserVM>().ReverseMap();
            CreateMap<ApplicationUser, CreateUserVM>().ReverseMap();
            CreateMap<IdentityRole, RoleVM>().ReverseMap();
            CreateMap<ChangeRoleVM, UserVM>().ReverseMap();




        }
    }
}
