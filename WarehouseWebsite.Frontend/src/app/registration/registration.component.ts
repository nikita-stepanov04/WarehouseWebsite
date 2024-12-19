import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthService} from '../auth/auth.service';
import {Router} from '@angular/router';
import {FormHelperService} from '../forms/form-helper.service';
import { RegisterInfo } from '../auth/register-info';
import {ErrorService} from '../error/error.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  registerForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private errorService: ErrorService,
    public fh: FormHelperService
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(32)]],
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(32)]],
      surname: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(32)]]
    });
  }

  onSubmit(): void {
    const formValue = this.registerForm.value;
    this.authService.register(
      new RegisterInfo(
        formValue.email,
        formValue.password,
        formValue.name,
        formValue.surname)
    ).subscribe({
      next: () => this.router.navigate(['/login']),
      error: (err) => this.errorService.handle(err.error.message)
    });
  }
}
