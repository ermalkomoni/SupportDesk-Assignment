export enum Department {
  Technical = 0,
  Billing = 1,
  General = 2,
}

export interface Agent {
  id: string;
  fullName: string;
  email: string;
  department: Department;
  isActive: boolean;
  assignedTicketCount: number;
}
