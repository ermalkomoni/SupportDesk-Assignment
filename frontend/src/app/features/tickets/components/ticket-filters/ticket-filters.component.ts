import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  inject,
} from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { catchError, of, shareReplay } from 'rxjs';

import { AgentService } from '../../../agents/services/agent.service';
import { TicketPriority } from '../../models/ticket-priority.enum';
import { TicketStatus } from '../../models/ticket-status.enum';

@Component({
  selector: 'sd-ticket-filters',
  standalone: true,
  imports: [AsyncPipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './ticket-filters.component.html',
})
export class TicketFiltersComponent {
  private readonly agentService = inject(AgentService);

  @Input() status: TicketStatus | null = null;
  @Input() priority: TicketPriority | null = null;
  @Input() assignedAgentId: string | null = null;
  @Input() overdueOnly = false;

  @Output() readonly statusChange = new EventEmitter<TicketStatus | null>();
  @Output() readonly priorityChange = new EventEmitter<TicketPriority | null>();
  @Output() readonly assignedAgentIdChange = new EventEmitter<string | null>();
  @Output() readonly overdueOnlyChange = new EventEmitter<boolean>();
  @Output() readonly clearFilters = new EventEmitter<void>();

  protected readonly agents$ = this.agentService.list().pipe(
    catchError(() => of([])),
    shareReplay({ bufferSize: 1, refCount: true }),
  );

  protected readonly statuses = [
    { value: TicketStatus.New, label: 'New' },
    { value: TicketStatus.InProgress, label: 'In progress' },
    { value: TicketStatus.Resolved, label: 'Resolved' },
    { value: TicketStatus.Closed, label: 'Closed' },
  ];

  protected readonly priorities = [
    { value: TicketPriority.Low, label: 'Low' },
    { value: TicketPriority.Normal, label: 'Normal' },
    { value: TicketPriority.High, label: 'High' },
    { value: TicketPriority.Critical, label: 'Critical' },
  ];

  protected onStatusChange(value: string): void {
    this.statusChange.emit(value === '' ? null : Number(value) as TicketStatus);
  }

  protected onPriorityChange(value: string): void {
    this.priorityChange.emit(value === '' ? null : Number(value) as TicketPriority);
  }

  protected onAgentChange(value: string): void {
    this.assignedAgentIdChange.emit(value || null);
  }
}
