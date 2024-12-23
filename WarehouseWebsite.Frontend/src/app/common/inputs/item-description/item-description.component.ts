import { Component } from '@angular/core';
import {BaseInputComponent} from '../base-input-component';

@Component({
  selector: 'app-item-description',
  templateUrl: './item-description.component.html',
  styleUrl: './item-description.component.css'
})
export class ItemDescriptionComponent extends BaseInputComponent {}
