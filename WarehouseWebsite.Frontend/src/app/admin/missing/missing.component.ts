import {Component, OnInit} from '@angular/core';
import {Item} from '../../shopping/item';
import {Pagination} from '../../common/pagination/pagination';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {ErrorService} from '../../error/error.service';
import {map} from 'rxjs/operators';
import {MissingItem} from './missing-item';

@Component({
  selector: 'app-missing',
  templateUrl: './missing.component.html'
})
export class MissingComponent implements OnInit{
  public items: Item[] = [];

  public pagination: Pagination = new Pagination(1, 8, 0);
  public nextDisabled: boolean = false;

  constructor(
    private http: HttpClient,
    private errorService: ErrorService) {}

  ngOnInit() {
    this.loadItems();
  }

  loadItems() {
    this.http.get<MissingItem[]>(`${environment.apiBasePath}/items/missing?page=${this.pagination.page}&count=${this.pagination.count}`)
      .pipe(
        map(missingItems => missingItems.map(missingItem => {
          const item = missingItem.item;
          item.quantity = missingItem.missing;
          return item;
        }))
      )
      .subscribe({
        next: (items) => {
          this.items = items;
          this.nextDisabled = this.items.length === 0;
        },
        error: (err) => this.errorService.handle(err)
      });
  }


  public previous(): void {
    if (this.pagination.page > 1) {
      this.pagination.page--;
      this.loadItems();
    }
  }

  public next(): void {
    if (!this.nextDisabled) {
      this.pagination.page++;
      this.loadItems();
    }
  }
}
