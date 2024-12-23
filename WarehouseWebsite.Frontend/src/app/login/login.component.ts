import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {FormHelperService} from '../forms/form-helper.service';
import {AuthService} from '../auth/auth.service';
import {LoginInfo} from '../auth/login-info';
import {Router} from '@angular/router';
import {ErrorService} from '../error/error.service';
import {TokenStorageService} from '../auth/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  loginForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private tokenService: TokenStorageService,
    private errorService: ErrorService,
    public fh: FormHelperService
  ) {
    this.tokenService.clearTokens();
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
        next: () => {
          this.router.navigate(['/home']);
          this.errorService.handleSuccess('Successfully logged in');
        },
        error: (err) => this.errorService.handle(err)
      });
  }
}
