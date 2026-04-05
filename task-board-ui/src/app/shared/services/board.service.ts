import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BoardService {
private api = `${environment.apiUrl}/auth`;
  //private api = 'https://localhost:7297/api/tasks';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<any[]>(this.api);
  }

  create(data: any) {
    return this.http.post<any>(this.api, data);
  }

  update(id: number, data: any) {
    return this.http.put<any>(`${this.api}/${id}`, data);
  }

  complete(id: number) {
    return this.http.put<any>(`${this.api}/${id}/complete`, {});
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
export class Board {}
