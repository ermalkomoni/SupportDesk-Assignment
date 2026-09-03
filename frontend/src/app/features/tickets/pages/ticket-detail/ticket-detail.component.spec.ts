import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';

import { AgentService } from '../../../agents/services/agent.service';
import { TicketPriority } from '../../models/ticket-priority.enum';
import { TicketStatus } from '../../models/ticket-status.enum';
import { TicketDetail } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { TicketDetailComponent } from './ticket-detail.component';

describe('TicketDetailComponent', () => {
  let ticketService: jasmine.SpyObj<TicketService>;
  let fixture: ComponentFixture<TicketDetailComponent>;

  beforeEach(async () => {
    ticketService = jasmine.createSpyObj<TicketService>('TicketService', [
      'getById',
      'changeStatus',
      'assignAgent',
      'unassignAgent',
      'addComment',
      'delete',
    ]);
    const agentService = jasmine.createSpyObj<AgentService>('AgentService', [
      'list',
    ]);
    agentService.list.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      imports: [TicketDetailComponent],
      providers: [
        provideRouter([]),
        { provide: TicketService, useValue: ticketService },
        { provide: AgentService, useValue: agentService },
      ],
    }).compileComponents();
  });

  it('renders exactly the transition returned by the API', async () => {
    await renderTicket({
      status: TicketStatus.New,
      allowedTransitions: [TicketStatus.InProgress],
    });

    const transitionButtons = fixture.nativeElement.querySelectorAll(
      '[data-testid="status-transition"]',
    ) as NodeListOf<HTMLButtonElement>;

    expect(transitionButtons.length).toBe(1);
    expect(transitionButtons[0].textContent?.trim()).toBe('In progress');
  });

  it('renders a closed ticket without mutable controls', async () => {
    await renderTicket({
      status: TicketStatus.Closed,
      allowedTransitions: [],
      closedDate: '2026-09-03T12:00:00Z',
    });

    const host = fixture.nativeElement as HTMLElement;
    const agentSelect = host.querySelector('#agent') as HTMLSelectElement;
    const deleteButton = Array.from(host.querySelectorAll('button')).find(
      (button) => button.textContent?.trim() === 'Delete ticket',
    ) as HTMLButtonElement;

    expect(host.querySelectorAll('[data-testid="status-transition"]').length).toBe(0);
    expect(host.querySelector('sd-comment-thread form')).toBeNull();
    expect(agentSelect.disabled).toBeTrue();
    expect(deleteButton.disabled).toBeTrue();
  });

  it('opens the delete confirmation dialog before deleting', async () => {
    await renderTicket({ status: TicketStatus.New });

    const host = fixture.nativeElement as HTMLElement;
    const deleteButton = Array.from(host.querySelectorAll('button')).find(
      (button) => button.textContent?.trim() === 'Delete ticket',
    ) as HTMLButtonElement;

    deleteButton.click();
    fixture.detectChanges();

    expect(host.querySelector('sd-confirm-dialog')).not.toBeNull();
    expect(ticketService.delete).not.toHaveBeenCalled();

    const cancelButton = Array.from(host.querySelectorAll('button')).find(
      (button) => button.textContent?.trim() === 'Cancel',
    ) as HTMLButtonElement;
    cancelButton.click();
    fixture.detectChanges();

    expect(host.querySelector('sd-confirm-dialog')).toBeNull();
  });

  async function renderTicket(
    overrides: Partial<TicketDetail>,
  ): Promise<void> {
    ticketService.getById.and.returnValue(of(createTicket(overrides)));
    fixture = TestBed.createComponent(TicketDetailComponent);
    fixture.componentRef.setInput('id', 'ticket-1');
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();
  }
});

function createTicket(overrides: Partial<TicketDetail>): TicketDetail {
  return {
    id: 'ticket-1',
    reference: 'TCK-2026-0001',
    title: 'Unable to sign in',
    description: 'The customer cannot access their account.',
    customerName: 'Acme Ltd',
    customerEmail: 'support@acme.test',
    priority: TicketPriority.Normal,
    status: TicketStatus.New,
    assignedAgentId: 'agent-1',
    assignedAgentName: 'Alex Agent',
    createdDate: '2026-09-01T12:00:00Z',
    lastModifiedDate: '2026-09-01T12:00:00Z',
    dueDate: '2026-09-04T12:00:00Z',
    resolvedDate: null,
    closedDate: null,
    isOverdue: false,
    allowedTransitions: [TicketStatus.InProgress],
    comments: [],
    ...overrides,
  };
}
