import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, map, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../environment';

export interface TokenResponseModel {
 
    token: string;
    refreshToken: string;
    userId: number;
    roles: {
      $id: string;
      $values: string[]; // The actual array of roles is inside the $values property
    };
  
}
export interface AppResponseModel<T> {
  data: T;
  isSuccess: boolean;
  message: string;
  recordsEffected: number;
  statusCode: number;
  success: boolean;
  totalRecords: number;
}
export interface AppResponseModelExt<T> {
  success: boolean;
  isSuccess: boolean;
  message: string;
  statusCode: number;
  data: {
    $id: string;
    $values: T;
  };
  recordsEffected: number;
  totalRecords: number;
}
// export interface AppResponseModel{
//   data: TokenResponseModel;
//   isSuccess: boolean;
//   message: string;
//   recordsEffected: number;
//   statusCode: number;
//   success: boolean;
//   totalRecords: number;
// }

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private url = environment.apiUrl+'/Account';// "https://localhost:7066/api/User";
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
   
      this.currentUserSubject = new BehaviorSubject<number | null>(null);
      this.currentUser = this.currentUserSubject.asObservable();
    
    this.checkTokenExpiration();
  }
  

  login(loginData: any): Observable<AppResponseModel<TokenResponseModel>> {
    return this.http.post<AppResponseModel<TokenResponseModel>>(`${this.url}/login`, loginData).pipe(
      map((response: AppResponseModel<TokenResponseModel>) => {
        console.log('authservice token:', response);
        this.setUserRoles(response);
        this.storeToken(response.data);
        return response;
      })
    );
  }
  setUserRoles(response: AppResponseModel<TokenResponseModel>): void {
    // Extract the roles array from the $values property
    const roles = response.data.roles.$values;
    
    // Store the roles array in localStorage as a JSON string
    localStorage.setItem('roles', JSON.stringify(roles));
  }
  
  getUserRoles(): string[] {
    const rolesString = localStorage.getItem('roles');
    return rolesString ? JSON.parse(rolesString) : [];
  }
  
  hasUserRole(role: string): boolean {
    const roles = this.getUserRoles();
    return roles.includes(role); // Check if the role exists in the roles array
  }
  
  register(user: any): Observable<any> {
    return this.http.post<any>(`${this.url}/register`, user);
  }

  public storeToken(response: TokenResponseModel) {
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
    this.router.navigate(['/register']);  // Redirect to the register page after logout
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

  public checkTokenExpiration(): void {
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

  public getTokenExpirationDate(token: string): Date | null {
    const decoded = this.decodeToken(token);
    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  private decodeToken(token: string): any {
    try {
      return JSON.parse(atob(token.split('.')[1]));
    } catch (error) {
      return null;
    }
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