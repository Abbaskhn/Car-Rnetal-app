import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authinterceptorInterceptor } from './authinterceptor.interceptor';
import { PermissionsService } from './guards/auth.guard';
import { AuthService } from './auth.service';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    PermissionsService,
    AuthService,
     provideAnimationsAsync(),provideHttpClient(withInterceptors([authinterceptorInterceptor]))]
};

