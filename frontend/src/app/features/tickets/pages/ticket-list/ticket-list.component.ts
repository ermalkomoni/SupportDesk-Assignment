import { AsyncPipe, DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
} from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Params, Router, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  Observable,
  catchError,
  debounceTime,
  distinctUntilChanged,
  map,
  of,
  shareReplay,
  startWith,
  switchMap,
  tap,
} from 'rxjs';

import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { TicketFiltersComponent } from '../../components/ticket-filters/ticket-filters.component';
import { TicketPriorityBadgeComponent } from '../../components/ticket-priority-badge/ticket-priority-badge.component';
import { TicketStatusBadgeComponent } from '../../components/ticket-status-badge/ticket-status-badge.component';
import { PagedResult } from '../../models/paged-result.model';
import { TicketPriority } from '../../models/ticket-priority.enum';
import { TicketQuery } from '../../models/ticket-query.model';
import { TicketStatus } from '../../models/ticket-status.enum';
import { TicketListItem } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';

interface TicketListState {
  result: PagedResult<TicketListItem> | null;
  loading: boolean;
  error: string | null;
}

@Component({
  selector: 'sd-ticket-list',
  standalone: true,
  imports: [
    AsyncPipe,
    DatePipe,
    PaginationComponent,
    ReactiveFormsModule,
    RouterLink,
    TicketFiltersComponent,
    TicketPriorityBadgeComponent,
    TicketStatusBadgeComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './ticket-list.component.html',
})
export class TicketListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly ticketService = inject(TicketService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly search = new FormControl('', { nonNullable: true });

  protected readonly query$ = this.route.queryParams.pipe(
    map((params) => this.toTicketQuery(params)),
    tap((query) =>
      this.search.setValue(query.search ?? '', { emitEvent: false }),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  private readonly state$: Observable<TicketListState> = this.query$.pipe(
    switchMap((query) =>
      this.ticketService.list(query).pipe(
        map((result) => ({ result, loading: false, error: null })),
        catchError((error: unknown) =>
          of({
            result: null,
            loading: false,
            error: this.errorMessage(error),
          }),
        ),
        startWith({ result: null, loading: true, error: null }),
      ),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly result$ = this.state$.pipe(
    map((state) => state.result),
    shareReplay({ bufferSize: 1, refCount: true }),
  );
  protected readonly loading$ = this.state$.pipe(map((state) => state.loading));
  protected readonly error$ = this.state$.pipe(map((state) => state.error));
  protected readonly skeletonRows = Array.from({ length: 6 });

  constructor() {
    this.search.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((term) => {
        void this.navigateWith({ search: term.trim() || null, page: 1 });
      });
  }

  protected changeStatus(status: TicketStatus | null): void {
    void this.navigateWith({ status, page: 1 });
  }

  protected changePriority(priority: TicketPriority | null): void {
    void this.navigateWith({ priority, page: 1 });
  }

  protected changeAgent(agent: string | null): void {
    void this.navigateWith({ agent, page: 1 });
  }

  protected changeOverdue(overdue: boolean): void {
    void this.navigateWith({ overdue: overdue ? true : null, page: 1 });
  }

  protected clearFilters(): void {
    this.search.setValue('', { emitEvent: false });
    void this.navigateWith({
      search: null,
      status: null,
      priority: null,
      agent: null,
      overdue: null,
      page: 1,
    });
  }

  protected changePage(page: number): void {
    void this.navigateWith({ page });
  }

  protected openTicket(id: string): void {
    void this.router.navigate(['/tickets', id]);
  }

  private navigateWith(queryParams: Params): Promise<boolean> {
    return this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge',
    });
  }

  private toTicketQuery(params: Params): TicketQuery {
    return {
      pageNumber: this.positiveInteger(params['page'], 1),
      pageSize: 20,
      search: this.nonEmptyString(params['search']),
      status: this.enumValue(params['status'], [
        TicketStatus.New,
        TicketStatus.InProgress,
        TicketStatus.Resolved,
        TicketStatus.Closed,
      ]),
      priority: this.enumValue(params['priority'], [
        TicketPriority.Low,
        TicketPriority.Normal,
        TicketPriority.High,
        TicketPriority.Critical,
      ]),
      assignedAgentId: this.nonEmptyString(params['agent']),
      overdueOnly: params['overdue'] === 'true' ? true : undefined,
    };
  }

  private positiveInteger(value: unknown, fallback: number): number {
    const parsed = Number(value);
    return Number.isInteger(parsed) && parsed > 0 ? parsed : fallback;
  }

  private nonEmptyString(value: unknown): string | undefined {
    return typeof value === 'string' && value.trim() ? value.trim() : undefined;
  }

  private enumValue<T extends string>(
    value: unknown,
    allowed: readonly T[],
  ): T | undefined {
    return typeof value === 'string' && allowed.includes(value as T)
      ? value as T
      : undefined;
  }

  private errorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'detail' in error &&
      typeof error.detail === 'string'
    ) {
      return error.detail;
    }

    return 'Tickets could not be loaded. Please try again.';
  }
}
