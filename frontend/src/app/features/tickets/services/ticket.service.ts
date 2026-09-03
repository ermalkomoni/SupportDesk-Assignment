import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { PagedResult } from '../models/paged-result.model';
import {
  AddCommentRequest,
  ChangeStatusRequest,
  CreateTicketRequest,
  UpdateTicketRequest,
} from '../models/ticket-requests.model';
import { TicketDetail, TicketListItem } from '../models/ticket.model';
import { TicketQuery } from '../models/ticket-query.model';

@Injectable({ providedIn: 'root' })
export class TicketService {
  private readonly http = inject(HttpClient);
  private readonly ticketsUrl = `${environment.apiBaseUrl}/Tickets`;

  list(query: TicketQuery): Observable<PagedResult<TicketListItem>> {
    return this.http.get<PagedResult<TicketListItem>>(this.ticketsUrl, {
      params: this.buildQueryParams(query),
    });
  }

  getById(id: string): Observable<TicketDetail> {
    return this.http.get<TicketDetail>(`${this.ticketsUrl}/${id}`);
  }

  create(request: CreateTicketRequest): Observable<TicketDetail> {
    return this.http.post<TicketDetail>(this.ticketsUrl, request);
  }

  update(id: string, request: UpdateTicketRequest): Observable<TicketDetail> {
    return this.http.put<TicketDetail>(`${this.ticketsUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.ticketsUrl}/${id}`);
  }

  changeStatus(id: string, request: ChangeStatusRequest): Observable<TicketDetail> {
    return this.http.put<TicketDetail>(`${this.ticketsUrl}/${id}/status`, request);
  }

  assignAgent(id: string, agentId: string): Observable<TicketDetail> {
    return this.http.put<TicketDetail>(`${this.ticketsUrl}/${id}/assign`, { agentId });
  }

  unassignAgent(id: string): Observable<TicketDetail> {
    return this.http.delete<TicketDetail>(`${this.ticketsUrl}/${id}/assign`);
  }

  addComment(
    id: string,
    request: AddCommentRequest,
  ): Observable<TicketDetail['comments'][number]> {
    return this.http.post<TicketDetail['comments'][number]>(
      `${this.ticketsUrl}/${id}/comments`,
      request,
    );
  }

  private buildQueryParams(query: TicketQuery): HttpParams {
    let params = new HttpParams()
      .set('PageNumber', query.pageNumber)
      .set('PageSize', query.pageSize);

    const search = query.search?.trim();
    if (search) {
      params = params.set('Search', search);
    }

    if (query.status !== undefined) {
      params = params.set('Status', query.status);
    }

    if (query.priority !== undefined) {
      params = params.set('Priority', query.priority);
    }

    const assignedAgentId = query.assignedAgentId?.trim();
    if (assignedAgentId) {
      params = params.set('AssignedAgentId', assignedAgentId);
    }

    if (query.overdueOnly !== undefined) {
      params = params.set('OverdueOnly', query.overdueOnly);
    }

    return params;
  }
}
