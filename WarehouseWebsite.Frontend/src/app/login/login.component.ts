import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {FormHelperService} from '../forms/form-helper.service';
import {AuthService} from '../auth/auth.service';
import {LoginInfo} from '../auth/login-info';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    public fh: FormHelperService
  ) {
    this.authService.logout();
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(32)]],
    });
  }

  onSubmit(): void {
    const formValue = this.loginForm.value;
    this.authService
      .logIn(new LoginInfo(formValue.email, formValue.password))
      .subscribe({
        next: () => this.router.navigate(['/home']),
        error: (err) => console.log(err.error.message)
      });
  }
}
