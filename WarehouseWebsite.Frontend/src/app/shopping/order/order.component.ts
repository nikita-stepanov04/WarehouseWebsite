import {Component, Input} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Order} from './order';
import {ErrorService} from '../../error/error.service';
import {Pagination} from '../../common/pagination/pagination';
import {ItemsCacheService} from '../item-caching.service';
import {Item} from '../item';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html'
})
export class OrderComponent {
  @Input() status: string = 'Transiting';

  public orders: Order[] = [];
  public collapsed: boolean = true;
  public pagination: Pagination = new Pagination(1, 1, 0);
  public nextDisabled: boolean = false;

  constructor(
    private http: HttpClient,
    private errorService: ErrorService,
    public itemService: ItemsCacheService) {}

  toggle() {
    if (this.collapsed) {
      this.loadOrders();
      this.collapsed = false;
    } else {
      this.collapsed = true;
      this.pagination.page = 1;
    }
  }

  getItem(itemId: string, quantity: number, price: number): Item | null {
    let item: Item | null = null;
    this.itemService.getItem(itemId).subscribe({
      next: (data) => {
        item = { ...data } as Item;
        item.quantity = quantity;
        item.price = price;
      },
      error: (err) => this.errorService.handle(err)
    });
    return item;
  }


  loadOrders() {
    this.http.get(`${environment.apiBasePath}/orders?status=${this.status}&page=${this.pagination.page}&count=${this.pagination.count}`)
      .subscribe({
        next: (orders) => {
          this.orders = orders as Order[]
          this.nextDisabled = this.orders.length == 0;
        },
        error: (err) => this.errorService.handle(err)
      });
  }

  public previous(): void {
    if (this.pagination.page > 1) {
      this.pagination.page--;
      this.loadOrders();
    }
  }

  public next(): void {
    if (!this.nextDisabled) {
      this.pagination.page++;
      this.loadOrders();
    }
  }
}
