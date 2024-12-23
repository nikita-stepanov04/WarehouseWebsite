import {Component, EventEmitter, Input, Output} from '@angular/core';
import {CartService} from '../cart.service';
import {Item} from '../item';
import {Router} from '@angular/router';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html'
})
export class CartItemComponent {
  @Input() item!: Item;
  @Output() remove = new EventEmitter<string>();

  public currentUrl: string = '';

  constructor(
    private cartService: CartService,
    private router: Router) {
    this.currentUrl = this.router.url;
  }

  onQuantityChange(value: string): void {
    const newQuantity = parseInt(value, 10);
    if (!isNaN(newQuantity) && newQuantity >= 1 && newQuantity <= 999) {
      this.item.quantity = newQuantity;
      this.cartService.setQuantity(this.item.id, newQuantity);
    }
  }
}
