import {Component, Input} from '@angular/core';
import {Item} from '../item';
import {CartService} from '../cart.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html'
})
export class ItemComponent {
  @Input() item!: Item;

  constructor(
    public cartService: CartService) {}
}
