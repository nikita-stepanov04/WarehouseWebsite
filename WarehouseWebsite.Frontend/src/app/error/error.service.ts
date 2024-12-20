import { Injectable } from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  error$ = new Subject<string>();
  success$ = new Subject<string>();

  handle(message: string) {
    this.error$.next(message);
  }

  handleSuccess(message: string) {
    this.success$.next(message);
  }

  clear() {
    this.error$.next('');
  }

  clearSuccess() {
    this.success$.next('');
  }
}
