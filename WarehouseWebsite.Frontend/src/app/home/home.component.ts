import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Item } from '../shopping/item';
import { environment } from '../../environments/environment';
import { ErrorService } from '../error/error.service';
import { Pagination } from '../common/pagination/pagination';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public items: Item[] = [];
  public pagination = new Pagination(1, 8, 0);
  public nextDisabled = false;

  constructor(
    private errorService: ErrorService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.http.get(`${environment.apiBasePath}/items?page=${this.pagination.page}&count=${this.pagination.count}`)
      .subscribe({
        next: (items) => {
          this.items = items as Item[];
          this.nextDisabled = this.items.length === 0;
        },
        error: (err) => this.errorService.handle(err)
      });
  }

  previous(): void {
    if (this.pagination.page > 1) {
      this.pagination.page--;
      this.loadItems();
    }
  }

  next(): void {
    this.pagination.page++;
    this.loadItems();
  }
}
