import { Injectable } from '@angular/core';
import { OrderLine } from './order-line';
import { Item } from './item';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ErrorService } from '../error/error.service';
import {ItemsCacheService} from './item-caching.service';

const CART_STORAGE = window.localStorage;
const CART_STORAGE_KEY = 'cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  cart: OrderLine[] = [];

  constructor(
    private errorService: ErrorService,
    private itemsCacheService: ItemsCacheService
  ) {
    this.loadCart();
  }

  public addItem(itemId: string, quantity: number): void {
    const item = this.cart.find(oi => oi.itemId === itemId);
    if (item) {
      item.quantity += quantity;
    } else {
      this.cart.push(new OrderLine(itemId, quantity));
    }
    this.saveCart();
  }

  public addItemFromInput(item: Item, input: HTMLInputElement): void {
    input.blur();
    let quantity = parseInt(input.value);

    if (quantity < 1) {
      input.value = '1';
      quantity = 1;
    } else if (quantity > 999) {
      input.value = '999';
      quantity = 999;
    }

    this.addItem(item.id, quantity);
    this.errorService.handleSuccess(`Successfully added ${item.name} to the cart in the quantity of ${quantity}`);
  }

  public removeItem(itemId: string): void {
    this.cart = this.cart.filter(oi => oi.itemId !== itemId);
    this.saveCart();
  }

  public setQuantity(itemId: string, quantity: number): void {
    const item = this.cart.find(oi => oi.itemId === itemId);
    if (item) {
      item.quantity = quantity;
    }
    this.saveCart();
  }

  public emptyCart(): void {
    CART_STORAGE.setItem(CART_STORAGE_KEY, '');
    this.cart = [];
  }

  public cartQuantityShorten(): string {
    const quantity = this.cart.reduce((accumulator, currentValue) => accumulator + currentValue.quantity, 0);
    return quantity > 99 ? '99+' : `${quantity}`;
  }

  public getCartItems(): Observable<Item[]> {
    const itemIds = this.cart.map(orderLine => orderLine.itemId);

    return this.itemsCacheService.getItems(itemIds).pipe(
      map(items => {
        items.forEach(item => {
          item.quantity = this.cart.find(orderLine => orderLine.itemId === item.id)!.quantity;
        });
        return items;
      })
    );
  }

  private saveCart(): void {
    CART_STORAGE.setItem(CART_STORAGE_KEY, JSON.stringify(this.cart));
  }

  private loadCart(): void {
    const cartStr = CART_STORAGE.getItem(CART_STORAGE_KEY);
    if (!cartStr) {
      this.cart = [];
    } else {
      this.cart = JSON.parse(cartStr);
    }
  }
}
