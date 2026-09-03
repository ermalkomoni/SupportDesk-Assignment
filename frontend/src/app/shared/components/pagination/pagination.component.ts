import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

@Component({
  selector: 'sd-pagination',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <nav class="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between" aria-label="Pagination">
      <p class="text-sm text-neutral-400">
        Showing {{ firstItem }} to {{ lastItem }} of {{ totalCount }}
      </p>

      <div class="flex items-center gap-2">
        <button
          type="button"
          class="rounded-lg border border-neutral-800 bg-neutral-900 px-4 py-2 text-sm font-medium text-neutral-200 transition-colors hover:border-neutral-700 hover:bg-neutral-800 focus:outline-none focus:ring-2 focus:ring-neutral-600/40 disabled:cursor-not-allowed disabled:opacity-40"
          [disabled]="page <= 1"
          (click)="pageChange.emit(page - 1)"
        >
          Previous
        </button>
        <span class="px-2 text-sm text-neutral-400">
          Page {{ page }} of {{ totalPages }}
        </span>
        <button
          type="button"
          class="rounded-lg border border-sky-500 bg-sky-500 px-4 py-2 text-sm font-semibold text-white transition-colors hover:bg-sky-400 focus:outline-none focus:ring-2 focus:ring-sky-500/40 disabled:cursor-not-allowed disabled:border-neutral-800 disabled:bg-neutral-900 disabled:text-neutral-500 disabled:opacity-100"
          [disabled]="page >= totalPages"
          (click)="pageChange.emit(page + 1)"
        >
          Next
        </button>
      </div>
    </nav>
  `,
})
export class PaginationComponent {
  @Input({ required: true }) page!: number;
  @Input({ required: true }) totalPages!: number;
  @Input({ required: true }) totalCount!: number;
  @Input() pageSize = 10;
  @Output() readonly pageChange = new EventEmitter<number>();

  protected get firstItem(): number {
    return this.totalCount === 0 ? 0 : (this.page - 1) * this.pageSize + 1;
  }

  protected get lastItem(): number {
    return Math.min(this.page * this.pageSize, this.totalCount);
  }
}
