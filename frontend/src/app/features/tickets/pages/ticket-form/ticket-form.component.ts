import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'sd-ticket-form',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '<h1 class="text-2xl font-semibold">Ticket Form</h1>',
})
export class TicketFormComponent {}
