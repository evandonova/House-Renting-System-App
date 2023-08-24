using AutoMapper;
using HouseRentingSystem.Web.Models.Houses;
using HouseRentingSystem.Services.Houses.Models;

namespace HouseRentingSystem.Web.Infrastructure
{
    public class ControllerMappingProfile : Profile
    {
        public ControllerMappingProfile()
        {
            this.CreateMap<HouseDetailsServiceModel, HouseFormModel>();
            this.CreateMap<HouseDetailsServiceModel, HouseDetailsViewModel>();
        }
    }
}
