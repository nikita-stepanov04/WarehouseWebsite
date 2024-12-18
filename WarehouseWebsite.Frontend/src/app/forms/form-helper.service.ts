import { Injectable } from '@angular/core';
import {AbstractControl} from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class FormHelperService {
  isInvalid(control: AbstractControl | null): boolean {
    return !!control?.invalid && (control?.dirty || control?.touched);
  }

  hasError(control: AbstractControl | null, validator: string): boolean {
    return !!control?.hasError(validator) && (control?.dirty || control?.touched);
  }
}
