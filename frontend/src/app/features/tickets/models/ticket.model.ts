import { TicketPriority } from './ticket-priority.enum';
import { TicketStatus } from './ticket-status.enum';

export interface TicketListItem {
  id: string;
  reference: string;
  title: string;
  customerName: string;
  priority: TicketPriority;
  status: TicketStatus;
  assignedAgentId: string | null;
  assignedAgentName: string | null;
  createdDate: string;
  dueDate: string;
  isOverdue: boolean;
}

export interface TicketDetail extends TicketListItem {
  description: string;
  customerEmail: string;
  lastModifiedDate: string;
  resolvedDate: string | null;
  closedDate: string | null;
  comments: TicketComment[];
}

export interface TicketComment {
  id: string;
  authorName: string;
  body: string;
  createdDate: string;
}
