import {Component, OnInit} from '@angular/core';
import {TokenStorageService} from '../auth/token-storage.service';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  info: any;

  constructor(private tokenService: TokenStorageService, private http: HttpClient) {}

  ngOnInit(): void {
    this.info = {
      accessToken: this.tokenService.getAccessToken(),
      refreshToken: this.tokenService.getRefreshToken(),
      roles: this.tokenService.getUserRoles()
    };
  }

  logout() {
    this.tokenService.logout();
    window.location.reload();
  }

  test() {
    this.http.get(`${environment.apiBasePath}/items/missing`, {})
      .subscribe(res => console.log(`Missing: ${res}`));
  }
}
