import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'sd-root',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="min-h-screen bg-neutral-950 text-neutral-100">
      <header class="border-b border-neutral-800 bg-neutral-900">
        <nav class="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <a routerLink="/tickets" class="text-lg font-semibold tracking-tight">
            SupportDesk
          </a>
          <a
            routerLink="/tickets"
            class="text-sm text-neutral-300 transition-colors hover:text-white"
          >
            Tickets
          </a>
        </nav>
      </header>

      <main class="mx-auto max-w-6xl px-6 py-8">
        <router-outlet />
      </main>
    </div>
  `,
})
export class AppComponent {}
