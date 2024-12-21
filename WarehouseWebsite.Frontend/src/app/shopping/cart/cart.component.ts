import { Component, OnInit } from '@angular/core';
import { Item } from '../item';
import { Pagination } from '../../common/pagination/pagination';
import { CartService } from '../cart.service';
import {ErrorService} from '../../error/error.service';
import {ModalService} from '../../common/modal/modal.service';
import {TokenStorageService} from '../../auth/token-storage.service';
import {Router} from '@angular/router';
import {Order} from '../order/order';
@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  public allItems: Item[] = [];
  public items: Item[] = [];
  public pagination = new Pagination(1, 3, 1);

  public orders: Order[] = [];

  constructor(
    public cartService: CartService,
    private errorService: ErrorService,
    private modalService: ModalService,
    public tokenService: TokenStorageService,
    private router: Router) {}

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.cartService.getCartItems().subscribe(allItems => {
      this.allItems = allItems;
      const startIndex = (this.pagination.page - 1) * this.pagination.count;
      const endIndex = startIndex + this.pagination.count;

      this.items = allItems.slice(startIndex, endIndex);
      this.pagination.totalPages = Math.ceil(allItems.length / this.pagination.count);
    });
  }

  public previous(): void {
    if (this.pagination.page > 1) {
      this.pagination.page--;
      this.loadItems();
    }
  }

  public next(): void {
    if (this.pagination.page < this.pagination.totalPages) {
      this.pagination.page++;
      this.loadItems();
    }
  }

  public removeItemFromCart(itemId: string) {
    const itemName = this.items.find(i => i.id === itemId)?.name
    this.modalService.openModal(`Remove ${itemName} from cart?`)
      .subscribe(result => {
        if (result) {
          this.cartService.removeItem(itemId);
          this.errorService.handleSuccess(
            `Successfully removed item ${itemName}`);
          this.loadItems();
        }
      });
  }

  public totalPrice() {
    return this.allItems.reduce((accumulator, item) =>
        accumulator + item.quantity * item.price, 0);
  }

  public purchase() {
    if (this.cartService.cart.length == 0) {
      this.errorService.handleSuccess('Nothing to purchase');
    }
    else if (this.tokenService.isAuthorized()) {
      this.router.navigate(['/purchase']);
    } else {
      this.errorService.handleSuccess('Please log in to purchase');
    }
  }

}
