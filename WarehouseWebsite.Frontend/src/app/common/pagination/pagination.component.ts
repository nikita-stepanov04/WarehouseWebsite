import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Pagination} from './pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() pagination!: Pagination;
  @Input() nextDisabled!: boolean;
  @Output() previous = new EventEmitter();
  @Output() next = new EventEmitter();
}
