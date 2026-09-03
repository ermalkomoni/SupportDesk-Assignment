using AutoMapper;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Core.Entities;

namespace SupportDesk.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Agent, AgentDto>()
			.ForCtorParam("AssignedTicketCount", opt => opt.MapFrom(src => src.Tickets.Count));
	}
}
