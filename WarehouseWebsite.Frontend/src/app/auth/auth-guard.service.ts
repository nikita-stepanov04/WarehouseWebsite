import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import { TokenStorageService } from './token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private tokenService: TokenStorageService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot): boolean {

    const expectedRole = route.data['expectedRole'];

    if (this.tokenService.isInRole(expectedRole)) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
