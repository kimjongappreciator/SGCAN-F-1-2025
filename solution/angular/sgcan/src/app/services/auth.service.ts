import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, catchError } from 'rxjs';
import { LoginModel } from '../models/loginModel';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {}

  baseUrl = 'http://host.docker.internal:9010';

  handleError(error: HttpErrorResponse): Observable<any> {
    return throwError(()=>({code: error.status, error: error.error}));
  }
  login(user: LoginModel): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, user).pipe(catchError(this.handleError));
  }

  register(user: any): Observable<any>{
    return this.http.post(`${this.baseUrl}/register`, user).pipe(catchError(this.handleError));
  }


}
