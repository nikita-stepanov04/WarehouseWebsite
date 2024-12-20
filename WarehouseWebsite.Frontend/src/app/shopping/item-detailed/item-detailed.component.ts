import {Component, OnInit} from '@angular/core';
import {Item} from '../item';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {ActivatedRoute} from '@angular/router';
import {ErrorService} from '../../error/error.service';
import {CartService} from '../cart.service';

@Component({
  selector: 'app-item-detailed',
  templateUrl: './item-detailed.component.html'
})
export class ItemDetailedComponent implements OnInit{
  private itemId: string | null = null;
  public item: Item | null = null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private errorService: ErrorService,
    public cartService: CartService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => this.itemId = params.get('id'));

    this.http.get(`${environment.apiBasePath}/items/${this.itemId}`)
      .subscribe({
        next: (item) => this.item = item as Item,
        error: (err) => this.errorService.handle(err)
      });
  }
}
