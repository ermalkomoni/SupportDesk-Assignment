using AutoMapper;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Application.DTOs.Comments;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Core.Entities;

namespace SupportDesk.Application.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Agent, AgentDto>()
			.ForCtorParam("AssignedTicketCount", opt => opt.MapFrom(src => src.Tickets.Count));

		CreateMap<Ticket, TicketListItemDto>()
			.ForCtorParam("AssignedAgentName",
				opt => opt.MapFrom(src => src.AssignedAgent != null ? src.AssignedAgent.FullName : null));

		CreateMap<Ticket, TicketDetailDto>()
			.ForCtorParam("AssignedAgentName",
				opt => opt.MapFrom(src => src.AssignedAgent != null ? src.AssignedAgent.FullName : null))
			.ForCtorParam("Comments",
				opt => opt.MapFrom(src => src.Comments.OrderBy(c => c.CreatedDate)));

		CreateMap<Comment, CommentDto>();
	}
}
