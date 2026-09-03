import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'sd-ticket-detail',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '<h1 class="text-2xl font-semibold">Ticket Detail</h1>',
})
export class TicketDetailComponent {}
