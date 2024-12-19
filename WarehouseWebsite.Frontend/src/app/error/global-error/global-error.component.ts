import {Component, OnInit} from '@angular/core';
import {ErrorService} from '../error.service';
import {animate, style, transition, trigger} from '@angular/animations';

@Component({
  selector: 'app-global-error',
  templateUrl: './global-error.component.html',
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-in',
          style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-out',
        style({ opacity: 0 }))
      ])
    ])
  ]
})
export class GlobalErrorComponent implements OnInit {
  errorMessage: string = '';

  constructor(public errorService: ErrorService) {}

  ngOnInit() {
    this.errorService.error$.subscribe((message: string) => {
      this.errorMessage = message;
      if (message) {
        setTimeout(() => this.errorService.clear(), 3500);
      }
    });
  }
}
