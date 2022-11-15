using AutoMapper;
using Database.Models;
using Database.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Models;
using ViewModel.ViewModels;
using ViewModel.ViewModels.Master;
using ViewModel.ViewModels.Transaksi;

namespace MainProject.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();


            CreateMap<HelperTable, HelperTableVM>(MemberList.None);
            CreateMap<HelperTableVM, HelperTable>(MemberList.None);


            CreateMap<FileLog, FileLogVM>(MemberList.None);
           

            CreateMap<Company, CompanyVM>(MemberList.None);
            CreateMap<CompanyVM, JsonCompanyVM>(MemberList.None);
            CreateMap<Company, JsonCompanyVM>(MemberList.None);
            CreateMap<JsonCompanyVM, Company>(MemberList.None);
            CreateMap<JsonCompanyVM, CompanyVM>(MemberList.None);
            CreateMap<Company, CompanyVM>().ForMember(vm => vm.Code_Name, mp => mp.MapFrom(d => d.Code + "-" + d.Name));
            CreateMap<ProjectReferences, ProjectReferencesVM>()
                .ForMember(vm => vm.ProjectType, mp => mp.MapFrom(d => d.typeNavigation.name));

            CreateMap<ProjectReferences, JsonProjectReferencesVM>()
                .ForMember(vm => vm.ProjectType, mp => mp.MapFrom(d => d.typeNavigation.name));

            CreateMap<CompanyVM, Company>(MemberList.None);
            CreateMap<FileLogDetail, FileLogDetailVM>(MemberList.None);

            CreateMap<CompanyProfile,JsonCompanyProfileVM>(MemberList.None);
            CreateMap<CompanyProfileVM, JsonCompanyProfileVM>(MemberList.None);

            CreateMap<SocialMedia, JsonSocialMediaVM>(MemberList.None);
            CreateMap<SocialMediaVM, JsonSocialMediaVM>(MemberList.None);

            CreateMap<Showroom, JsonShowroomVM>(MemberList.None);
            CreateMap<ShowroomVM, JsonShowroomVM>(MemberList.None);

            CreateMap<JsonCatalogDetailVM, CatalogDetail>(MemberList.None);

            CreateMap<ProjectType, JsonProjectTypeVM>(MemberList.None);
            CreateMap<ProjectTypeVM, JsonProjectTypeVM>(MemberList.None);
             
            
            CreateMap<CatalogType, JsonCatalogTypeVM>(MemberList.None);
            CreateMap<CatalogTypeVM, JsonCatalogTypeVM>(MemberList.None);

            CreateMap<CatalogDetail, JsonCatalogTypeVM>(MemberList.None);
            CreateMap<CatalogDetailVM, JsonCatalogDetailVM>(MemberList.None);
            CreateMap<CatalogTypeVM, JsonCatalogTypeVM>(MemberList.None);

            CreateMap<CatalogDetail, JsonCatalogDetailVM>(MemberList.None);
        }
    }
}
