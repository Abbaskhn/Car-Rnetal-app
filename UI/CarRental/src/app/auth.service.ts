import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, map, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';

export interface TokenResponseModel {
  token: string;
  refreshToken: string;
  userId: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = "https://localhost:7066/api/User";
  private refreshTokenInProgress = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private currentTokenSubject: BehaviorSubject<string | null>;
  private currentUserSubject: BehaviorSubject<number | null>;
  public currentToken: Observable<string | null>;
  public currentUser: Observable<number | null>;

  constructor(private http: HttpClient, private router: Router) {
    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');
    this.currentTokenSubject = new BehaviorSubject<string | null>(storedToken ? storedToken : null);
    this.currentUserSubject = new BehaviorSubject<number | null>(storedUserId ? Number(storedUserId) : null);
    this.currentToken = this.currentTokenSubject.asObservable();
    this.currentUser = this.currentUserSubject.asObservable();
    this.checkTokenExpiration();
  }

  login(loginData: any): Observable<any> {
    return this.http.post<TokenResponseModel>(`${this.url}/authenticate`, loginData).pipe(
      map((response: TokenResponseModel) => {
        this.storeToken(response);
        return response;
      })
    );
  }

  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.url}/register`, user);
  }
  storeToken(response: TokenResponseModel) {
    console.log('Received response:', response);
  
    if (response.token) {
      localStorage.setItem('token', response.token);
      this.currentTokenSubject.next(response.token);
    }
  
    if (response.refreshToken) {
      localStorage.setItem('refreshToken', response.refreshToken);
    }
  
    if (response.userId !== undefined && response.userId !== null) {
      localStorage.setItem('userId', response.userId.toString());
      this.currentUserSubject.next(response.userId);
    } 
  }
  


  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserId(): number | null {
    return this.currentUserSubject.value;
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('userId');
    this.currentTokenSubject.next(null);
    this.currentUserSubject.next(null);
    this.router.navigate(['/register']);
  }

  refreshToken(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    if (refreshToken && !this.refreshTokenInProgress) {
      this.refreshTokenInProgress = true;
      return this.http.post<TokenResponseModel>(`${this.url}/refresh-token`, { refreshToken, token: this.getToken() || '' }).pipe(
        map((response: TokenResponseModel) => {
          this.refreshTokenInProgress = false;
          this.storeToken(response);
          this.refreshTokenSubject.next(response.token);
          return response.token;
        }),
        catchError(error => {
          this.refreshTokenInProgress = false;
          this.logout();
          return throwError(error);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1)
      );
    }
  }

  checkTokenExpiration(): void {
    const token = this.getToken();
    if (token) {
      const expirationDate = this.getTokenExpirationDate(token);
      const now = new Date();

      if (expirationDate && expirationDate < now) {
        this.refreshToken().subscribe(
          () => { },
          () => this.logout()
        );
      } else {
        const timeout = expirationDate ? expirationDate.getTime() - now.getTime() : 0;
        setTimeout(() => this.checkTokenExpiration(), timeout);
      }
    }
  }

  getTokenExpirationDate(token: string): Date | null {
    const decoded = this.decodeToken(token);
    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  decodeToken(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (error) {
      return null;
    }
  }

  // Role checks
  isAdmin(): boolean {
    return this.getCurrentUserRole() === 'Admin';
  }

  isVendor(): boolean {
    return this.getCurrentUserRole() === 'Vendor';
  }

  isCustomer(): boolean {
    return this.getCurrentUserRole() === 'Customer';
  }

  public getCurrentUserRole(): string | null {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.decodeToken(token);
      return decodedToken ? decodedToken.role : null;
    }
    return null;
  }
}
