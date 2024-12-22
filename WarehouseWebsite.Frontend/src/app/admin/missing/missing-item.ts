import {Item} from '../../shopping/item';

export class MissingItem {
  missing: number;
  item: Item;

  constructor(missing: number, item: Item) {
    this.missing = missing;
    this.item = item;
  }
}
