export enum Department {
  Technical = 'Technical',
  Billing = 'Billing',
  General = 'General',
}

export interface Agent {
  id: string;
  fullName: string;
  email: string;
  department: Department;
  isActive: boolean;
  assignedTicketCount: number;
}
