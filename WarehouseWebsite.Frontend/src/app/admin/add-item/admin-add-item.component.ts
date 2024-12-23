import { Component } from '@angular/core';
import {FormHelperService, fractionValidator} from '../../forms/form-helper.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {ErrorService} from '../../error/error.service';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-add-item',
  templateUrl: './admin-add-item.component.html'
})
export class AdminAddItemComponent {
  public addItemForm: FormGroup;

  constructor(
    public fh: FormHelperService,
    private fb: FormBuilder,
    private http: HttpClient,
    private errorService: ErrorService) {

    this.addItemForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]],
      quantity: ['', [Validators.required, Validators.min(0), Validators.max(1_000_000)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      price: ['', [Validators.required, fractionValidator, Validators.min(0.01), Validators.max(10_000_000)]],
      weight: ['', [Validators.required, fractionValidator, Validators.min(0.01), Validators.max(1000)]],
      category: ['', [Validators.required]],
      image: ['', [Validators.required]],
    });
  }

  onSubmit() {
    const formData = new FormData();
    Object.keys(this.addItemForm.controls).forEach(key => {
      formData.append(key, this.addItemForm.get(key)!.value);
    });
    this.http.post(`${environment.apiBasePath}/items`, formData)
      .subscribe({
        next: () => this.errorService.handleSuccess('Successfully added new Item'),
        error: (err) => this.errorService.handle(err)
      });
  }
}
