import { Injectable, inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../auth.service';

// export const authGuard: CanActivateFn = (route, state) => {
//   return true;

// };

@Injectable()
 export class PermissionsService {
  constructor(private http:AuthService,private router:Router){}

 canActivate(): boolean {
  if(this.http.isLoggedIn()){
    return true;
  }
  else{
    this.router.navigate(['login'])
    return false;
  }
}

 }



export const authGuard: CanActivateFn = (
  next: ActivatedRouteSnapshot,
  state: RouterStateSnapshot) => {
    return inject(PermissionsService).canActivate();
}