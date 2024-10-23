import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private apiUrl = `${environment.apiUrl}/api/inventory-items`;
  loading = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) {}

  getInventoryItems(params: any): Observable<any> {
    this.loading.next(true);
    return this.http.get<any>(this.apiUrl, { params: params }).pipe(
      tap(() => this.loading.next(false)),
      catchError((error) => {
        this.loading.next(false);
        return throwError(() => new Error(error));
      })
    );
  }

  getInventoryItemById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  addInventoryItem(item: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, item);
  }

  updateInventoryItem(id: string, item: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, item);
  }

  deleteInventoryItem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}
