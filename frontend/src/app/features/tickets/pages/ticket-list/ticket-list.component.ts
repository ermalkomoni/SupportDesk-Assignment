import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'sd-ticket-list',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '<h1 class="text-2xl font-semibold">Ticket List</h1>',
})
export class TicketListComponent {}
