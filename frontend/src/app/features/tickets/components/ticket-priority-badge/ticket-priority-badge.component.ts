import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { TicketPriority } from '../../models/ticket-priority.enum';

@Component({
  selector: 'sd-ticket-priority-badge',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <span
      class="inline-flex rounded-full px-2.5 py-1 text-xs font-medium"
      [class]="priorityClasses[priority]"
    >
      {{ priorityLabels[priority] }}
    </span>
  `,
})
export class TicketPriorityBadgeComponent {
  @Input({ required: true }) priority!: TicketPriority;

  protected readonly priorityLabels: Record<TicketPriority, string> = {
    [TicketPriority.Low]: 'Low',
    [TicketPriority.Normal]: 'Normal',
    [TicketPriority.High]: 'High',
    [TicketPriority.Critical]: 'Critical',
  };

  protected readonly priorityClasses: Record<TicketPriority, string> = {
    [TicketPriority.Low]:
      'bg-neutral-400/10 text-neutral-300 ring-1 ring-inset ring-neutral-400/20',
    [TicketPriority.Normal]:
      'bg-blue-400/10 text-blue-300 ring-1 ring-inset ring-blue-400/20',
    [TicketPriority.High]:
      'bg-orange-400/10 text-orange-300 ring-1 ring-inset ring-orange-400/20',
    [TicketPriority.Critical]:
      'bg-red-400/10 text-red-300 ring-1 ring-inset ring-red-400/20',
  };
}
