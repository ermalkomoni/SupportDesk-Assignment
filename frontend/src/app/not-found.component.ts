import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'sd-not-found',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: '<h1 class="text-2xl font-semibold">Not Found</h1>',
})
export class NotFoundComponent {}
