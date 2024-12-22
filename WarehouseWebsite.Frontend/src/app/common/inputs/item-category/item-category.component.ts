import { Component } from '@angular/core';
import {BaseInputComponent} from '../base-input-component';
import {ItemCategory} from '../../../shopping/item';

@Component({
  selector: 'app-item-category',
  templateUrl: './item-category.component.html',
  styleUrl: './item-category.component.css'
})
export class ItemCategoryComponent extends BaseInputComponent {
  itemKeys = Object.keys(ItemCategory) as Array<keyof typeof ItemCategory>;
  protected readonly ItemCategory = ItemCategory;
}
