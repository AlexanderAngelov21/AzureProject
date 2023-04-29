
namespace AleksandarWebDevelopment2Project.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Services.Cars.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Category, CarCategoryServiceModel>();

            this.CreateMap<FuelType, CarFuelTypeServiceModel>();

            this.CreateMap<TransmissionType, CarTransmissionTypeServiceModel>();

            this.CreateMap<ExtraType, CarExtrasServiceModel>();
        }
    }
}
