import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { TicketStatus } from '../../models/ticket-status.enum';

@Component({
  selector: 'sd-ticket-status-badge',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <span
      class="inline-flex rounded-full px-2.5 py-1 text-xs font-medium"
      [class]="statusClasses[status]"
    >
      {{ statusLabels[status] }}
    </span>
  `,
})
export class TicketStatusBadgeComponent {
  @Input({ required: true }) status!: TicketStatus;

  protected readonly statusLabels: Record<TicketStatus, string> = {
    [TicketStatus.New]: 'New',
    [TicketStatus.InProgress]: 'In progress',
    [TicketStatus.Resolved]: 'Resolved',
    [TicketStatus.Closed]: 'Closed',
  };

  protected readonly statusClasses: Record<TicketStatus, string> = {
    [TicketStatus.New]: 'bg-sky-400/10 text-sky-300 ring-1 ring-inset ring-sky-400/20',
    [TicketStatus.InProgress]:
      'bg-amber-400/10 text-amber-300 ring-1 ring-inset ring-amber-400/20',
    [TicketStatus.Resolved]:
      'bg-emerald-400/10 text-emerald-300 ring-1 ring-inset ring-emerald-400/20',
    [TicketStatus.Closed]:
      'bg-neutral-400/10 text-neutral-300 ring-1 ring-inset ring-neutral-400/20',
  };
}
