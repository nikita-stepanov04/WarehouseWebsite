import { Injectable } from '@angular/core';
import {Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  error$ = new Subject<string>();
  success$ = new Subject<string>();

  handle(error: any) {
    const message = error?.error?.message;
    const status = error?.status;
    if (message) {
      this.error$.next(message);
    } else if (status) {
      this.error$.next(`Error ${status}`);
    } else {
      this.error$.next('Unexpected error happened')
    }
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
