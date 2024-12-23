import { Component } from '@angular/core';
import {ModalService} from './modal.service';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html'
})
export class ModalComponent {
  modalText: string = '';

  constructor(public modalService: ModalService) {
    modalService.modal$.subscribe(modalText => this.modalText = modalText);
  }

  onAccept() {
    this.modalService.accept();
    this.modalService.modal$.next('');
  }

  onReject() {
    this.modalService.reject();
    this.modalService.modal$.next('');
  }
}
