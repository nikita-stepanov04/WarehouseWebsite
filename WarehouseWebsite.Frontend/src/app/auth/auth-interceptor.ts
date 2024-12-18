import {
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import {TokenStorageService} from './token-storage.service';
import {Injectable} from '@angular/core';
import {catchError, Observable, switchMap} from 'rxjs';
import {AuthService} from './auth.service';
import {TokenInfo} from './token-info';

const TOKEN_HEADER_KEY = 'Authorization';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private token: TokenStorageService, private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;
    if (this.token.isAuthorized()) {
      authReq = req.clone({headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${this.token.getAccessToken()}`)});
      return next.handle(authReq).pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            let tokenInfo = new TokenInfo(
              this.token.getAccessToken(),
              this.token.getRefreshToken()
            );
            return this.authService.refresh(tokenInfo).pipe(
              switchMap((tokenInfo: TokenInfo) => {
                const newAuthRequest = req.clone({headers: req.headers.set(TOKEN_HEADER_KEY, `Bearer ${tokenInfo.accessToken}`)});
                return next.handle(newAuthRequest);
              }),
              catchError(err => {
                throw new Error(err.message);
              })
            );
          } else {
            throw new Error(error.message);
          }
        })
      );
    }
    return next.handle(authReq);
  }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
]
