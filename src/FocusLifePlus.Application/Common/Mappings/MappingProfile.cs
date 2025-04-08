using AutoMapper;
using FocusLifePlus.Application.Features.FocusTasks.Queries.GetFocusTaskById;
using FocusLifePlus.Domain.Entities;

namespace FocusLifePlus.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FocusTask, FocusTaskDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.FocusCategory.Name));
        }
    }
} 