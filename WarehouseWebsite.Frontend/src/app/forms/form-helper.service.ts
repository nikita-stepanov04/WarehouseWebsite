import { Injectable } from '@angular/core';
import {AbstractControl, ValidationErrors} from '@angular/forms';

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

export function fractionValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (value == null || value === '') {
    return null;
  }
  const isValid = !isNaN(value) && Number(value) === parseFloat(value);
  return isValid ? null : { 'invalidFraction': true };
}
