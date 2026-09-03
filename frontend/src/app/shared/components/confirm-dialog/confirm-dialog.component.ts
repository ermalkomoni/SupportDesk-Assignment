import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  HostListener,
  Input,
  Output,
} from '@angular/core';

@Component({
  selector: 'sd-confirm-dialog',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/70 px-4"
      role="presentation"
      (click)="onBackdropClick($event)"
    >
      <section
        class="w-full max-w-md rounded-xl border border-neutral-700 bg-neutral-900 p-6 shadow-2xl"
        role="dialog"
        aria-modal="true"
        [attr.aria-labelledby]="dialogTitleId"
      >
        <h2 class="text-lg font-semibold text-neutral-100" [id]="dialogTitleId">
          {{ title }}
        </h2>
        <p class="mt-3 text-sm leading-6 text-neutral-400">{{ message }}</p>

        <div class="mt-6 flex justify-end gap-3">
          <button
            type="button"
            class="rounded-lg border border-neutral-700 px-4 py-2 text-sm font-medium text-neutral-200 transition-colors hover:bg-neutral-800"
            (click)="cancel.emit()"
          >
            Cancel
          </button>
          <button
            type="button"
            class="rounded-lg bg-red-600 px-4 py-2 text-sm font-semibold text-white transition-colors hover:bg-red-500"
            (click)="confirm.emit()"
          >
            {{ confirmLabel }}
          </button>
        </div>
      </section>
    </div>
  `,
})
export class ConfirmDialogComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) message!: string;
  @Input({ required: true }) confirmLabel!: string;
  @Output() readonly confirm = new EventEmitter<void>();
  @Output() readonly cancel = new EventEmitter<void>();

  protected readonly dialogTitleId = 'confirm-dialog-title';

  @HostListener('document:keydown.escape')
  protected onEscape(): void {
    this.cancel.emit();
  }

  protected onBackdropClick(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.cancel.emit();
    }
  }
}
