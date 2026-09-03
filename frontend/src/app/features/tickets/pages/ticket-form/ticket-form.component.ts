import { ChangeDetectionStrategy, Component, Input, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { ApiError } from '../../../../core/http/problem-details.model';
import { TicketPriority } from '../../models/ticket-priority.enum';
import { TicketService } from '../../services/ticket.service';

@Component({
  selector: 'sd-ticket-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="mx-auto max-w-3xl">
      <a routerLink="/tickets" class="text-sm font-medium text-sky-400 hover:text-sky-300">
        &larr; Back to tickets
      </a>

      <header class="mt-6 border-b border-neutral-800 pb-6">
        <p class="mb-2 text-xs font-medium uppercase tracking-wide text-neutral-500">
          {{ isEditing() ? 'Update request' : 'New request' }}
        </p>
        <h1 class="text-3xl font-semibold tracking-tight text-white">
          {{ isEditing() ? 'Edit ticket' : 'Create ticket' }}
        </h1>
        <p class="mt-2 text-sm text-neutral-400">
          Capture the customer, priority, and request details in one place.
        </p>
      </header>

      @if (loadError(); as error) {
        <div class="mt-6 rounded-xl border border-red-900/70 bg-red-950/40 p-4 text-sm text-red-200" role="alert">
          {{ error }}
        </div>
      }

      <form class="mt-6 space-y-6" [formGroup]="form" (ngSubmit)="submit()">
        <section class="rounded-xl border border-neutral-800 bg-neutral-900/80 p-5 shadow-2xl shadow-black/20">
          <h2 class="text-base font-semibold text-neutral-100">Ticket details</h2>

          <div class="mt-5 grid gap-5">
            <label class="grid gap-1.5 text-sm font-medium text-neutral-300">
              Title
              <input
                type="text"
                formControlName="title"
                class="rounded-lg border border-neutral-700 bg-neutral-950/60 px-3 py-2.5 text-sm font-normal text-neutral-100 outline-none transition placeholder:text-neutral-500 focus:border-sky-500 focus:ring-2 focus:ring-sky-500/20"
                placeholder="Short summary of the issue"
              />
              @if (showError('title')) {
                <span class="text-xs text-red-300">Title is required.</span>
              }
            </label>

            <label class="grid gap-1.5 text-sm font-medium text-neutral-300">
              Description
              <textarea
                rows="6"
                formControlName="description"
                class="resize-y rounded-lg border border-neutral-700 bg-neutral-950/60 px-3 py-2.5 text-sm font-normal leading-6 text-neutral-100 outline-none transition placeholder:text-neutral-500 focus:border-sky-500 focus:ring-2 focus:ring-sky-500/20"
                placeholder="What happened, what the customer expected, and any useful context"
              ></textarea>
              @if (showError('description')) {
                <span class="text-xs text-red-300">Description is required.</span>
              }
            </label>

            <label class="grid gap-1.5 text-sm font-medium text-neutral-300 sm:max-w-xs">
              Priority
              <select
                formControlName="priority"
                class="h-11 rounded-lg border border-neutral-700 bg-neutral-950/60 px-3 text-sm font-normal text-neutral-100 outline-none transition focus:border-sky-500 focus:ring-2 focus:ring-sky-500/20"
              >
                @for (priority of priorities; track priority.value) {
                  <option [value]="priority.value">{{ priority.label }}</option>
                }
              </select>
            </label>
          </div>
        </section>

        <section class="rounded-xl border border-neutral-800 bg-neutral-900/80 p-5 shadow-2xl shadow-black/20">
          <h2 class="text-base font-semibold text-neutral-100">Customer</h2>

          <div class="mt-5 grid gap-5 sm:grid-cols-2">
            <label class="grid gap-1.5 text-sm font-medium text-neutral-300">
              Name
              <input
                type="text"
                formControlName="customerName"
                class="rounded-lg border border-neutral-700 bg-neutral-950/60 px-3 py-2.5 text-sm font-normal text-neutral-100 outline-none transition placeholder:text-neutral-500 focus:border-sky-500 focus:ring-2 focus:ring-sky-500/20"
                placeholder="Customer name"
              />
              @if (showError('customerName')) {
                <span class="text-xs text-red-300">Customer name is required.</span>
              }
            </label>

            <label class="grid gap-1.5 text-sm font-medium text-neutral-300">
              Email
              <input
                type="email"
                formControlName="customerEmail"
                class="rounded-lg border border-neutral-700 bg-neutral-950/60 px-3 py-2.5 text-sm font-normal text-neutral-100 outline-none transition placeholder:text-neutral-500 focus:border-sky-500 focus:ring-2 focus:ring-sky-500/20"
                placeholder="customer@example.com"
              />
              @if (showError('customerEmail')) {
                <span class="text-xs text-red-300">Enter a valid customer email.</span>
              }
            </label>
          </div>
        </section>

        @if (submitError(); as error) {
          <div class="rounded-xl border border-red-900/70 bg-red-950/40 p-4 text-sm text-red-200" role="alert">
            {{ error }}
          </div>
        }

        <div class="flex flex-col-reverse gap-3 sm:flex-row sm:justify-end">
          <a
            routerLink="/tickets"
            class="inline-flex items-center justify-center rounded-lg border border-neutral-700 px-4 py-2.5 text-sm font-medium text-neutral-200 transition-colors hover:border-neutral-600 hover:bg-neutral-800"
          >
            Cancel
          </a>
          <button
            type="submit"
            class="inline-flex items-center justify-center rounded-lg bg-sky-500 px-4 py-2.5 text-sm font-semibold text-white shadow-lg shadow-sky-950/40 transition-colors hover:bg-sky-400 focus:outline-none focus:ring-2 focus:ring-sky-500/40 disabled:cursor-not-allowed disabled:bg-neutral-800 disabled:text-neutral-500 disabled:shadow-none"
            [disabled]="saving()"
          >
            {{ saving() ? 'Saving...' : (isEditing() ? 'Save changes' : 'Create ticket') }}
          </button>
        </div>
      </form>
    </div>
  `,
})
export class TicketFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly ticketService = inject(TicketService);

  protected readonly ticketId = signal<string | null>(null);
  protected readonly saving = signal(false);
  protected readonly loadError = signal<string | null>(null);
  protected readonly submitError = signal<string | null>(null);

  protected readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(4000)]],
    customerName: ['', [Validators.required, Validators.maxLength(200)]],
    customerEmail: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    priority: [TicketPriority.Normal, Validators.required],
  });

  protected readonly priorities = [
    { value: TicketPriority.Low, label: 'Low' },
    { value: TicketPriority.Normal, label: 'Normal' },
    { value: TicketPriority.High, label: 'High' },
    { value: TicketPriority.Critical, label: 'Critical' },
  ];

  @Input() set id(value: string | undefined) {
    this.ticketId.set(value ?? null);

    if (value) {
      this.loadTicket(value);
    }
  }

  protected isEditing(): boolean {
    return this.ticketId() !== null;
  }

  protected showError(controlName: keyof typeof this.form.controls): boolean {
    const control = this.form.controls[controlName];
    return control.invalid && (control.dirty || control.touched);
  }

  protected submit(): void {
    this.submitError.set(null);

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);

    const id = this.ticketId();
    const request = this.form.getRawValue();
    const save$ = id
      ? this.ticketService.update(id, request)
      : this.ticketService.create(request);

    save$.pipe(finalize(() => this.saving.set(false))).subscribe({
      next: (ticket) => {
        void this.router.navigate(['/tickets', ticket.id]);
      },
      error: (error: unknown) => {
        this.submitError.set(this.errorMessage(error, 'Ticket could not be saved. Please try again.'));
      },
    });
  }

  private loadTicket(id: string): void {
    this.loadError.set(null);
    this.ticketService.getById(id).subscribe({
      next: (ticket) => {
        this.form.setValue({
          title: ticket.title,
          description: ticket.description,
          customerName: ticket.customerName,
          customerEmail: ticket.customerEmail,
          priority: ticket.priority,
        });
      },
      error: (error: unknown) => {
        this.loadError.set(this.errorMessage(error, 'Ticket could not be loaded.'));
      },
    });
  }

  private errorMessage(error: unknown, fallback: string): string {
    return error instanceof ApiError ? error.detail || error.title : fallback;
  }
}
