import { TicketPriority } from './ticket-priority.enum';
import { TicketStatus } from './ticket-status.enum';

export interface TicketQuery {
  pageNumber: number;
  pageSize: number;
  search?: string;
  status?: TicketStatus;
  priority?: TicketPriority;
  assignedAgentId?: string;
  overdueOnly?: boolean;
}
