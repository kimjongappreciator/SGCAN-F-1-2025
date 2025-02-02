import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { FileModel } from '../models/fileModel';
import { detailsModel } from '../models/detailsModel';

@Injectable({
  providedIn: 'root'
})
export class ScrapService {

  constructor(private http: HttpClient) { }

  baseUrl = 'http://localhost:5000';

  handleError(error: HttpErrorResponse): Observable<any> {
    return throwError(()=>({code: error.status, error: error.error}));
  }

  getFilesByUserId(id: string): Observable<FileModel[]>{
    return this.http.get<FileModel[]>(`${this.baseUrl}/files/${id}`).pipe(catchError(this.handleError));
  }

  getUrlsByFileId(id: string): Observable<detailsModel[]>{
    return this.http.get<string[]>(`${this.baseUrl}/files/${id}/links`).pipe(catchError(this.handleError));
  }

}
