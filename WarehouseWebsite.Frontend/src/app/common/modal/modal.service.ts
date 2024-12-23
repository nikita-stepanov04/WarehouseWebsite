import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalResult$ = new Subject<boolean>();
  public modal$ = new Subject<string>();

  openModal(message: string) {
    this.modal$.next(message);
    return this.modalResult$.pipe(take(1));
  }

  accept() {
    this.modalResult$.next(true);
  }

  reject() {
    this.modalResult$.next(false);
  }
}
