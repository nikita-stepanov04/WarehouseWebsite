import { Injectable } from '@angular/core';
import {OrderLine} from './order-line';

const CART_STORAGE = window.localStorage;
const CART_STORAGE_KEY = 'cart'

@Injectable({
  providedIn: 'root'
})
export class CartService {

  cart: OrderLine[] = [];

  constructor() {
    this.loadCart();
  }

  public addItem(itemId: string, quantity: number) {
    const item = this.cart.find(oi => oi.itemId === itemId);
    if (item) {
      item.quantity += quantity;
    } else {
      this.cart.push(new OrderLine(itemId, 1));
    }
    this.saveCart();
  }

  public removeItem(itemId: string) {
     this.cart = this.cart.filter(oi => oi.itemId !== itemId);
     this.saveCart();
  }

  public setQuantity(itemId: string, quantity: number) {
    const item = this.cart.find(oi => oi.itemId == itemId);
    if (item) {
      item.quantity = quantity;
    }
    this.saveCart();
  }

  public cartQuantityShorten(): string {
    const quantity = this.cart.reduce((accumulator, currentValue) => accumulator + currentValue.quantity, 0);
    return quantity > 99 ? '99+' : `${quantity}`;
  }

  private saveCart() {
    CART_STORAGE.setItem(CART_STORAGE_KEY, JSON.stringify(this.cart));
  }

  private loadCart() {
    const cartStr = CART_STORAGE.getItem(CART_STORAGE_KEY);
    if (!cartStr){
      this.cart = [];
    } else {
      this.cart = JSON.parse(cartStr);
    }
  }
}
