import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { TokenStorageService } from './token-storage.service';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import {log} from '@angular-devkit/build-angular/src/builders/ssr-dev-server';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private tokenService: TokenStorageService, private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authToken = this.tokenService.getAccessToken();
    const authReq = req.clone({
      setHeaders: { Authorization: `Bearer ${authToken}` }
    });

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.authService.refresh({
            accessToken: this.tokenService.getAccessToken(),
            refreshToken: this.tokenService.getRefreshToken()
          }).pipe(
            switchMap((tokenInfo: any) => {
              const newToken = tokenInfo.accessToken;
              this.tokenService.saveAccessToken(newToken);
              this.tokenService.saveRefreshToken(tokenInfo.refreshToken);

              const newAuthReq = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` }
              });

              return next.handle(newAuthReq);
            }),
            catchError((refreshError: HttpErrorResponse) => {
              if (refreshError.status === 400) {
                this.router.navigate(['/login']);
                throw new Error();
              }
              throw refreshError;
            })
          );
        } else {
          throw error;
        }
      })
    );
  }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];
