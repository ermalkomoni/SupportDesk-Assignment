import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { TicketPriority } from '../../models/ticket-priority.enum';

@Component({
  selector: 'sd-ticket-priority-badge',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <span class="inline-flex items-center gap-2 text-sm font-medium text-neutral-200">
      <span
        class="h-2 w-2 rounded-full"
        [class]="priorityDotClasses[priority]"
        aria-hidden="true"
      ></span>
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

  protected readonly priorityDotClasses: Record<TicketPriority, string> = {
    [TicketPriority.Low]: 'bg-neutral-400',
    [TicketPriority.Normal]: 'bg-sky-500',
    [TicketPriority.High]: 'bg-amber-400',
    [TicketPriority.Critical]: 'bg-red-400',
  };
}
