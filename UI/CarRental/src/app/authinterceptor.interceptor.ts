import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authinterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Clone the request to add the new header if accessToken is present
  const accessToken = authService.getToken();
  let authReq = req;

  if (accessToken) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Handle token expiration error
      if (error.status === 401 && error.error.code === 'token_not_valid') {
        console.log('Token expired, attempting to refresh...');

        // Attempt to refresh the token
        return authService.refreshToken().pipe(
          switchMap((newToken: string) => {
            // Clone the request with the new access token
            const newAuthReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });
            // Retry the original request with the new token
            return next(newAuthReq);
          }),
          catchError((refreshError) => {
            console.error('Token refresh failed:', refreshError);
            authService.logout();  // Note the use of `logout()`
            router.navigate(['/login']);
            return throwError(() => new Error('Token refresh failed'));
          })
        );
      }
      return throwError(() => error);
    })
  );
};
