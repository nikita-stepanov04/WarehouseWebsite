import {Component, OnInit} from '@angular/core';
import {OrderComponent} from '../../shopping/order/order.component';
import {Pagination} from '../../common/pagination/pagination';
import {HttpClient} from '@angular/common/http';
import {ErrorService} from '../../error/error.service';
import {ItemsCacheService} from '../../shopping/item-caching.service';
import {ActivatedRoute} from '@angular/router';
import {ModalService} from '../../common/modal/modal.service';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-orders',
  templateUrl: './admin-orders.component.html'
})
export class AdminOrdersComponent extends OrderComponent implements OnInit {

  public override pagination: Pagination = new Pagination(1, 3, 0);

  constructor(
    http: HttpClient,
    errorService: ErrorService,
    itemService: ItemsCacheService,
    private route: ActivatedRoute,
    private modalService: ModalService) {
    super(http, errorService, itemService);
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.status = params.get('status') ?? 'Transiting';
      this.loadOrders();
    });
  }

  setTransited(orderId: string) {
    this.modalService.openModal(`Set order ${orderId} as transited?`)
      .subscribe(result => {
        if (result) {
          this.http.post(`${environment.apiBasePath}/orders/set-transited/${orderId}`, {})
            .subscribe({
              next: () => {
                this.errorService.handleSuccess(`Successfully set order ${orderId} as transited`);
                this.loadOrders();
              },
              error: (err) => this.errorService.handle(err)
            });
        }
      })
  }
}
