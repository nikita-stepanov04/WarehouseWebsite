import { Injectable } from '@angular/core';
import {Item} from './item';
import {HttpClient} from '@angular/common/http';
import {forkJoin, Observable, of} from 'rxjs';
import {environment} from '../../environments/environment';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ItemsCacheService {

  private items: { [id: string]: Item } = {};

  constructor(private http: HttpClient) {
    setTimeout(() => this.items = {}, 5 * 60 * 1000); // 5 minutes
  }

  getItem(itemId: string): Observable<Item> {
    if (this.items[itemId]) {
      return of(this.items[itemId]);
    }

    return this.http.get<Item>(`${environment.apiBasePath}/items/${itemId}`).pipe(
      map(item => {
        this.items[itemId] = item;
        return item;
      })
    );
  }

  getItems(itemIds: string[]): Observable<Item[]> {
    const items: Item[] = itemIds
      .filter(itemId => this.items[itemId])
      .map(itemId => this.items[itemId]);

    const itemsToFetch = itemIds.filter(itemId => !this.items[itemId]);

    if (itemsToFetch.length > 0) {
      return forkJoin(
        itemsToFetch.map(itemId => this.getItem(itemId))
      ).pipe(
        map(fetchedItems => {
          return [...items, ...fetchedItems];
        })
      );
    }

    return of(items);
  }
}
