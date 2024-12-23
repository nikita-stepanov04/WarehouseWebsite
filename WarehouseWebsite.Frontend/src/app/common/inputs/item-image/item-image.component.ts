import { Component } from '@angular/core';
import {BaseInputComponent} from '../base-input-component';

@Component({
  selector: 'app-item-image',
  templateUrl: './item-image.component.html',
  styleUrl: './item-image.component.css'
})
export class ItemImageComponent extends BaseInputComponent {
  onFileChange(event: any): void {
    const file = (event.target as HTMLInputElement).files![0];
    if (file) {
      this.parentForm.patchValue({
        [this.controlName]: file
      });
      this.parentForm.get(this.controlName)!.updateValueAndValidity();
    }
  }
}
