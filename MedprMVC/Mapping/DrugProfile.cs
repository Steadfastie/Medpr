using AutoMapper;
using MedprCore.DTO;
using MedprDB.Entities;

namespace MedprMVC.Mapping
{
    public class DrugProfile : Profile
    {
        public DrugProfile() 
        {
            CreateMap<Drug, DrugDTO>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(drug => drug.Id))
                .ForMember(dto => dto.Name, opt => opt.MapFrom(drug => drug.Name))
                .ForMember(dto => dto.PharmGroup, opt => opt.MapFrom(drug => drug.PharmGroup))
                .ForMember(dto => dto.Price, opt => opt.MapFrom(drug => drug.Price));
            CreateMap<DrugDTO, Drug>()
                .ForMember(drug => drug.Id, opt => opt.MapFrom(dto => dto.Id))
                .ForMember(drug => drug.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(drug => drug.PharmGroup, opt => opt.MapFrom(dto => dto.PharmGroup))
                .ForMember(drug => drug.Price, opt => opt.MapFrom(dto => dto.Price));
        }
    }
}
