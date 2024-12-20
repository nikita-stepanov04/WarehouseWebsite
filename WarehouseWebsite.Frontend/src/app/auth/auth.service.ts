import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {LoginInfo} from './login-info';
import {Observable, tap} from 'rxjs';
import {JwtResponse} from './jwt-response';
import {RegisterInfo} from './register-info';
import {TokenInfo} from './token-info';
import {TokenStorageService} from './token-storage.service';
import {environment} from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({'Content-Type': 'application/json'})
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl = `${environment.apiBasePath}/account/login`;
  private registerUrl = `${environment.apiBasePath}/account/register`;
  private refreshUrl = `${environment.apiBasePath}/account/refresh`;

  constructor(private http: HttpClient, private tokenService: TokenStorageService) { }

  logIn(credentials: LoginInfo): Observable<JwtResponse> {
    return this.http.post<JwtResponse>(this.loginUrl, credentials, httpOptions)
      .pipe(
        tap((data: JwtResponse) => {
          this.tokenService.saveAccessToken(data.accessToken);
          this.tokenService.saveRefreshToken(data.refreshToken);
          this.tokenService.saveUserRoles(data.roles);
        })
      );
  }

  register(info: RegisterInfo): Observable<string> {
    return this.http.post<string>(this.registerUrl, info, httpOptions);
  }

  refresh(info: TokenInfo): Observable<TokenInfo> {
    return this.http.post<TokenInfo>(this.refreshUrl, info, httpOptions)
      .pipe(
        tap((data: TokenInfo) => {
          this.tokenService.saveAccessToken(data.accessToken);
          this.tokenService.saveRefreshToken(data.refreshToken);
        })
      )
  }
}
