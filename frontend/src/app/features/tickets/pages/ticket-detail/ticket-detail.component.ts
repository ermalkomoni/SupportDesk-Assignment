import { AsyncPipe, DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  Input,
  inject,
  signal,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  BehaviorSubject,
  EMPTY,
  Observable,
  ReplaySubject,
  catchError,
  combineLatest,
  finalize,
  map,
  shareReplay,
  switchMap,
  tap,
} from 'rxjs';

import { ApiError } from '../../../../core/http/problem-details.model';
import { AgentService } from '../../../agents/services/agent.service';
import { CommentThreadComponent } from '../../components/comment-thread/comment-thread.component';
import { TicketPriorityBadgeComponent } from '../../components/ticket-priority-badge/ticket-priority-badge.component';
import { TicketStatusBadgeComponent } from '../../components/ticket-status-badge/ticket-status-badge.component';
import { TicketStatus } from '../../models/ticket-status.enum';
import { TicketService } from '../../services/ticket.service';

@Component({
  selector: 'sd-ticket-detail',
  standalone: true,
  imports: [
    AsyncPipe,
    CommentThreadComponent,
    DatePipe,
    RouterLink,
    TicketPriorityBadgeComponent,
    TicketStatusBadgeComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './ticket-detail.component.html',
})
export class TicketDetailComponent {
  private readonly ticketService = inject(TicketService);
  private readonly agentService = inject(AgentService);
  private readonly id$ = new ReplaySubject<string>(1);
  private readonly refresh$ = new BehaviorSubject<void>(undefined);

  @Input({ required: true }) set id(value: string) {
    this.id$.next(value);
  }

  protected readonly loading = signal(true);
  protected readonly loadError = signal<string | null>(null);
  protected readonly mutationError = signal<string | null>(null);
  protected readonly actionPending = signal(false);
  protected readonly ticketStatus = TicketStatus;

  protected readonly activeAgents$ = this.agentService.list().pipe(
    map((agents) => agents.filter((agent) => agent.isActive)),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly ticket$ = combineLatest([this.id$, this.refresh$]).pipe(
    tap(() => {
      this.loading.set(true);
      this.loadError.set(null);
    }),
    switchMap(([id]) =>
      this.ticketService.getById(id).pipe(
        catchError((error: unknown) => {
          this.loadError.set(this.errorMessage(error, 'Ticket could not be loaded.'));
          return EMPTY;
        }),
        finalize(() => this.loading.set(false)),
      ),
    ),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected changeStatus(id: string, status: TicketStatus): void {
    this.runMutation(this.ticketService.changeStatus(id, { newStatus: status }));
  }

  protected assignAgent(id: string, agentId: string): void {
    if (agentId) {
      this.runMutation(this.ticketService.assignAgent(id, agentId));
    }
  }

  protected unassignAgent(id: string): void {
    this.runMutation(this.ticketService.unassignAgent(id));
  }

  protected addComment(
    id: string,
    comment: { authorName: string; body: string },
  ): void {
    this.runMutation(this.ticketService.addComment(id, comment));
  }

  protected statusLabel(status: TicketStatus): string {
    return status === TicketStatus.InProgress ? 'In progress' : status;
  }

  private runMutation(request: Observable<unknown>): void {
    this.mutationError.set(null);
    this.actionPending.set(true);

    request.pipe(finalize(() => this.actionPending.set(false))).subscribe({
      next: () => this.refresh$.next(),
      error: (error: unknown) =>
        this.mutationError.set(
          this.errorMessage(error, 'The action could not be completed.'),
        ),
    });
  }

  private errorMessage(error: unknown, fallback: string): string {
    return error instanceof ApiError ? error.detail || error.title : fallback;
  }
}
