import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth/auth.service';
import {TokenStorageService} from '../auth/token-storage.service';
import {LoginInfo} from '../auth/login-info';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  loggedIn: boolean = false;
  roles: string[] = [];
  form: any = {};

  constructor(private authService: AuthService, private tokenService: TokenStorageService) {}

  ngOnInit(): void {
    if (this.tokenService.getAccessToken()) {
      this.loggedIn = true;
      this.roles = this.tokenService.getUserRoles();
    }
  }

  onSubmit() {
    let loginInfo = new LoginInfo(this.form.email, this.form.password);
    this.authService.logIn(loginInfo).subscribe({
      next: data => {
        this.loggedIn = true;
        console.log(data.accessToken)
        this.roles = data.roles;
      },
      error: error => {
        console.log(error.error.message);
      }
    });
  }
}
