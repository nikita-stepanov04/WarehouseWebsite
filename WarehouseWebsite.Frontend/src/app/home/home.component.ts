import {Component, OnInit} from '@angular/core';
import {TokenStorageService} from '../auth/token-storage.service';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  info: any;

  constructor(
    public tokenService: TokenStorageService,
    private http: HttpClient) {}

  ngOnInit(): void {
    this.info = {
      accessToken: this.tokenService.getAccessToken(),
      refreshToken: this.tokenService.getRefreshToken(),
      roles: this.tokenService.getUserRoles()
    };
  }
}
