import { Pipe, PipeTransform } from '@angular/core';
import { ItemCategory } from './item';

@Pipe({
  name: 'itemCategory'
})
export class ItemCategoryPipe implements PipeTransform {
  private readonly categoryMap = new Map<ItemCategory, string>([
    [ItemCategory.None, 'No Category'],
    [ItemCategory.Electronics, 'Electronics & Gadgets'],
    [ItemCategory.HomeGoods, 'Home & Furniture'],
    [ItemCategory.Clothing, 'Apparel'],
    [ItemCategory.BuildingMaterials, 'Construction Materials'],
    [ItemCategory.Books, 'Literature & Books']
  ]);

  transform(value: ItemCategory): string {
    return this.categoryMap.get(value) || value;
  }
}
