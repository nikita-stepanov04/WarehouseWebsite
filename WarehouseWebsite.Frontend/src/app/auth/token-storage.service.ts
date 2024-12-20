import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {ErrorService} from '../error/error.service';

const TOKEN_KEY = "AccessToken";
const REFRESH_TOKEN_KEY = "RefreshToken";
const USER_ROLE_KEY = "UserRole";

const TOKEN_STORE = window.localStorage;

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  constructor(private router: Router, private errorService: ErrorService) {}

  public saveAccessToken(token: string) {
    TOKEN_STORE.removeItem(TOKEN_KEY);
    TOKEN_STORE.setItem(TOKEN_KEY, token);
  }

  public getAccessToken(): string {
    return TOKEN_STORE.getItem(TOKEN_KEY) ?? "";
  }

  public saveRefreshToken(token: string) {
    TOKEN_STORE.removeItem(REFRESH_TOKEN_KEY);
    TOKEN_STORE.setItem(REFRESH_TOKEN_KEY, token);
  }

  public getRefreshToken(): string {
    return TOKEN_STORE.getItem(REFRESH_TOKEN_KEY) ?? "";
  }

  public saveUserRoles(userRoles: Array<string>) {
    TOKEN_STORE.removeItem(USER_ROLE_KEY);
    TOKEN_STORE.setItem(USER_ROLE_KEY, JSON.stringify(userRoles));
  }

  public getUserRoles(): Array<string> {
    let roles = TOKEN_STORE.getItem(USER_ROLE_KEY);
    if (roles !== null) {
      return JSON.parse(roles);
    }
    return []
  }

  public logout() {
    this.clearTokens();
    const currentUrl = this.router.url;
    if (this.isAuthEndpoint(currentUrl)) {
      this.router.navigate(['/login']);
    }
    this.errorService.handleSuccess('Successfully logged out');
  }

  public clearTokens() {
    TOKEN_STORE.setItem(TOKEN_KEY, '');
    TOKEN_STORE.setItem(REFRESH_TOKEN_KEY, '');
    TOKEN_STORE.setItem(USER_ROLE_KEY, '');
  }

  public isAuthorized(): boolean {
    const token = TOKEN_STORE.getItem(TOKEN_KEY);
    return token !== null && token !== '';
  }

  public isInRole(role: string): boolean {
    const roles = TOKEN_STORE.getItem(USER_ROLE_KEY);
    if (roles === null) return false;
    const rolesArray = JSON.parse(roles) as string[];
    return rolesArray.includes(role);
  }

  private isAuthEndpoint(currentUrl: string) {
    var routes = this.router.config
      .filter(route => route.data && route.data['expectedRole'])
      .map(route => `/${route.path}`);
    console.log(routes);
    console.log(currentUrl);
    routes
      .some(route => route.startsWith(currentUrl));
    return false;
  }
}
