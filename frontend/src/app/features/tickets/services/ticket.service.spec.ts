import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { environment } from '../../../../environments/environment';
import { TicketStatus } from '../models/ticket-status.enum';
import { TicketService } from './ticket.service';

describe('TicketService', () => {
  let service: TicketService;
  let httpTestingController: HttpTestingController;
  const ticketsUrl = `${environment.apiBaseUrl}/Tickets`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TicketService,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service = TestBed.inject(TicketService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpTestingController.verify());

  it('sends the defined list query parameters and omits undefined filters', () => {
    service
      .list({
        pageNumber: 2,
        pageSize: 10,
        search: 'acme',
        status: TicketStatus.New,
        overdueOnly: true,
      })
      .subscribe();

    const request = httpTestingController.expectOne(
      (candidate) => candidate.url === ticketsUrl,
    );

    expect(request.request.method).toBe('GET');
    expect(request.request.params.keys().sort()).toEqual(
      ['OverdueOnly', 'PageNumber', 'PageSize', 'Search', 'Status'].sort(),
    );
    expect(request.request.params.get('PageNumber')).toBe('2');
    expect(request.request.params.get('PageSize')).toBe('10');
    expect(request.request.params.get('Search')).toBe('acme');
    expect(request.request.params.get('Status')).toBe('New');
    expect(request.request.params.get('OverdueOnly')).toBe('true');
    expect(request.request.params.has('Priority')).toBeFalse();
    expect(request.request.params.has('AssignedAgentId')).toBeFalse();

    request.flush({
      items: [],
      pageNumber: 2,
      pageSize: 10,
      totalCount: 0,
      totalPages: 0,
      hasPrevious: true,
      hasNext: false,
    });
  });

  it('uses the dedicated status endpoint with the correct body', () => {
    const id = 'ticket-1';

    service
      .changeStatus(id, { newStatus: TicketStatus.InProgress })
      .subscribe();

    const request = httpTestingController.expectOne(`${ticketsUrl}/${id}/status`);

    expect(request.request.method).toBe('PUT');
    expect(request.request.body).toEqual({
      newStatus: TicketStatus.InProgress,
    });

    request.flush({});
  });
});
