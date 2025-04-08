using AutoMapper;
using FocusLifePlus.Domain.Entities;
using FocusLifePlus.Application.Features.Tasks.DTOs;
using System.Linq;

namespace FocusLifePlus.Application.Common.Mappings
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<FocusTask, TaskDetailsDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.FocusCategory != null ? src.FocusCategory.Name : null))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Username : null))
                .ForMember(dest => dest.SubTasks, opt => opt.MapFrom(src => src.SubTasks))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)))
                .ForMember(dest => dest.Reminders, opt => opt.MapFrom(src => src.Reminders));

            CreateMap<FocusTask, TaskListDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.FocusCategory != null ? src.FocusCategory.Name : null))
                .ForMember(dest => dest.SubTaskCount, opt => opt.MapFrom(src => src.SubTasks.Count))
                .ForMember(dest => dest.CompletedSubTaskCount, opt => opt.MapFrom(src => src.SubTasks.Count(st => st.IsCompleted)));

            CreateMap<FocusSubTask, SubTaskDto>();
            CreateMap<FocusReminder, ReminderDto>();
        }
    }
} 