import {Component, Input} from '@angular/core';
import {Item} from '../item';
import {CartService} from '../cart.service';
import {ErrorService} from '../../error/error.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrl: './item.component.css'
})
export class ItemComponent {
  @Input() item!: Item;

  constructor(
    private cartService: CartService,
    private errorService: ErrorService) {}

  addItem(input: HTMLInputElement) {
    input.blur();
    let quantity = parseInt(input.value);

    if (quantity < 1) {
      input.value = '1';
      quantity = 1;
    } else if (quantity > 999) {
      input.value = '999'
      quantity = 999;
    }

    this.cartService.addItem(this.item.id, quantity);
    this.errorService.handleSuccess(`Successfully added ${this.item.name} to the cart in the quantity of ${quantity}`);
  }
}
