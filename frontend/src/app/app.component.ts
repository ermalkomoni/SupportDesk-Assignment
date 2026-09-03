import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'sd-root',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="min-h-screen bg-neutral-950 text-neutral-100">
      <aside class="fixed inset-y-0 left-0 hidden w-64 border-r border-neutral-800 bg-neutral-950/95 px-4 py-5 lg:block">
        <a routerLink="/tickets" class="flex items-center px-2">
          <img
            src="/assets/pecb-logo.png"
            alt="PECB"
            class="h-8 w-auto"
          />
        </a>

        <nav class="mt-8 space-y-1" aria-label="Primary navigation">
          <a
            routerLink="/tickets"
            routerLinkActive="border-sky-500/30 bg-sky-500/10 text-white"
            class="flex items-center gap-3 rounded-lg border border-transparent px-3 py-2.5 text-sm font-medium text-neutral-300 transition-colors hover:bg-neutral-900 hover:text-white"
          >
            <span class="h-2 w-2 rounded-full bg-current"></span>
            Tickets
          </a>
        </nav>
      </aside>

      <header class="border-b border-neutral-800 bg-neutral-950/95 lg:hidden">
        <nav class="flex items-center justify-between px-4 py-4">
          <a routerLink="/tickets" class="flex items-center">
            <img
              src="/assets/pecb-logo.png"
              alt="PECB"
              class="h-7 w-auto"
            />
          </a>
          <a
            routerLink="/tickets"
            routerLinkActive="text-white"
            class="text-sm font-medium text-neutral-300 transition-colors hover:text-white"
          >
            Tickets
          </a>
        </nav>
      </header>

      <main class="px-4 py-6 sm:px-6 lg:ml-64 lg:px-12 lg:py-8">
        <router-outlet />
      </main>
    </div>
  `,
})
export class AppComponent {}
