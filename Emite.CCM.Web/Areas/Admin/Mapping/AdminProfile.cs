using AutoMapper;
using Emite.CCM.Web.Areas.Admin.Commands.Entities;
using Emite.CCM.Web.Areas.Admin.Models;
using Emite.Common.Data;
using Microsoft.AspNetCore.Identity;
using Emite.CCM.Core.Identity;

namespace Emite.CCM.Web.Areas.Admin.Mapping;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<Entity, EntityViewModel>().ReverseMap();
        CreateMap<EntityViewModel, AddOrEditEntityCommand>();
        CreateMap<AddOrEditEntityCommand, Entity>();

        CreateMap<ApplicationRole, RoleViewModel>().ReverseMap();

        CreateMap<Audit, AuditLogViewModel>();
        CreateMap<ApplicationUser, AuditLogUserViewModel>();
    }
}
