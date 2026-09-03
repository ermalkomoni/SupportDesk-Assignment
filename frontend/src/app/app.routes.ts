import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'tickets',
  },
  {
    path: 'tickets',
    loadComponent: () =>
      import('./features/tickets/pages/ticket-list/ticket-list.component').then(
        (component) => component.TicketListComponent,
      ),
  },
  {
    path: 'tickets/new',
    loadComponent: () =>
      import('./features/tickets/pages/ticket-form/ticket-form.component').then(
        (component) => component.TicketFormComponent,
      ),
  },
  {
    path: 'tickets/:id',
    loadComponent: () =>
      import('./features/tickets/pages/ticket-detail/ticket-detail.component').then(
        (component) => component.TicketDetailComponent,
      ),
  },
  {
    path: 'tickets/:id/edit',
    loadComponent: () =>
      import('./features/tickets/pages/ticket-form/ticket-form.component').then(
        (component) => component.TicketFormComponent,
      ),
  },
  {
    path: '**',
    loadComponent: () =>
      import('./not-found.component').then(
        (component) => component.NotFoundComponent,
      ),
  },
];
