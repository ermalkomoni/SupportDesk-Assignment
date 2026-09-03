import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { Agent } from '../models/agent.model';

@Injectable({ providedIn: 'root' })
export class AgentService {
  private readonly http = inject(HttpClient);
  private readonly agentsUrl = `${environment.apiBaseUrl}/Agents`;

  list(search?: string): Observable<Agent[]> {
    const value = search?.trim();
    const params = value ? new HttpParams().set('search', value) : undefined;

    return this.http.get<Agent[]>(this.agentsUrl, { params });
  }

  getById(id: string): Observable<Agent> {
    return this.http.get<Agent>(`${this.agentsUrl}/${id}`);
  }
}
