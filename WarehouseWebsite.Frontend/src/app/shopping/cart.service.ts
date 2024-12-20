import { Injectable} from '@angular/core';
import { OrderLine } from './order-line';
import { Item } from './item';
import { HttpClient } from '@angular/common/http';
import {forkJoin, Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {environment} from '../../environments/environment';
import {ErrorService} from '../error/error.service';

const CART_STORAGE = window.localStorage;
const CART_STORAGE_KEY = 'cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  cart: OrderLine[] = [];
  private items: { [id: string]: Item } = {};

  constructor(private http: HttpClient, private errorService: ErrorService) {
    this.loadCart();
    setTimeout(() => this.items = {}, 5 * 60 * 1000) // 5 minutes
  }

  public addItem(itemId: string, quantity: number) {
    const item = this.cart.find(oi => oi.itemId === itemId);
    if (item) {
      item.quantity += quantity;
    } else {
      this.cart.push(new OrderLine(itemId, quantity));
    }
    this.saveCart();
  }

  public addItemFromInput(item: Item, input: HTMLInputElement) {
    input.blur();
    let quantity = parseInt(input.value);

    if (quantity < 1) {
      input.value = '1';
      quantity = 1;
    } else if (quantity > 999) {
      input.value = '999'
      quantity = 999;
    }

    this.addItem(item.id, quantity);
    this.errorService.handleSuccess(`Successfully added ${item.name} to the cart in the quantity of ${quantity}`);
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

  public getCartItems(): Observable<Item[]> {
    const itemIds = this.cart.map(orderLine => orderLine.itemId);
    let items: Item[] = itemIds
      .filter(itemId => this.items[itemId])
      .map(itemId => this.items[itemId]);

    const itemsToFetch = itemIds.filter(itemId => !this.items[itemId]);

    if (itemsToFetch.length > 0) {
      const fetchRequests = itemsToFetch.map(itemId =>
        this.http.get<Item>(`${environment.apiBasePath}/items/${itemId}`).pipe(
          map(item => {
            this.items[itemId] = item;
            return item;
          })
        )
      );

      return forkJoin(fetchRequests).pipe(
        map(fetchedItems => {
          items = [...items, ...fetchedItems];
          items.forEach(item => {
            item.quantity = this.cart.find(orderLine => orderLine.itemId === item.id)!.quantity;
          });
          return items;
        })
      );
    } else {
      items.forEach(item => {
        item.quantity = this.cart.find(orderLine => orderLine.itemId === item.id)!.quantity;
      });
      return new Observable(observer => {
        observer.next(items);
        observer.complete();
      });
    }
  }

  private saveCart() {
    CART_STORAGE.setItem(CART_STORAGE_KEY, JSON.stringify(this.cart));
  }

  private loadCart() {
    const cartStr = CART_STORAGE.getItem(CART_STORAGE_KEY);
    if (!cartStr) {
      this.cart = [];
    } else {
      this.cart = JSON.parse(cartStr);
    }
  }
}
