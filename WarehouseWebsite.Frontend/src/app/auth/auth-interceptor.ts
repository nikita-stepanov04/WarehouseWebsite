import {
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { TokenStorageService } from './token-storage.service';
import { Injectable } from '@angular/core';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { TokenInfo } from './token-info';
import { Router } from '@angular/router';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(
    private token: TokenStorageService,
    private authService: AuthService,
    private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;
    if (this.token.isAuthorized()) {
      authReq = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${this.token.getAccessToken()}`) });
    }

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !this.isRefreshing) {
          this.isRefreshing = true;
          let tokenInfo = new TokenInfo(
            this.token.getAccessToken(),
            this.token.getRefreshToken()
          );
          return this.authService.refresh(tokenInfo).pipe(
            switchMap((newTokenInfo: TokenInfo) => {
              this.isRefreshing = false;
              const newAuthRequest = req.clone({ headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${newTokenInfo.accessToken}`) });
              return next.handle(newAuthRequest);
            }),
            catchError(err => {
              this.isRefreshing = false;
              this.token.clearTokens();
              this.router.navigate(['/login']);
              throw new Error(err);
            })
          );
        } else {
          this.router.navigate(['/login']);
          throw new Error();
        }
      })
    );
  }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];
