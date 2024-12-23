import { Component } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpClient} from '@angular/common/http';
import {ErrorService} from '../../error/error.service';
import {environment} from '../../../environments/environment';

@Component({
  selector: 'app-restock-item',
  templateUrl: './restock-item.component.html',
  styleUrl: './restock-item.component.css'
})
export class RestockItemComponent {
  restockForm: FormGroup

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private errorService: ErrorService) {
    this.restockForm = this.fb.group({
      id: ['', [Validators.required]],
      addQuantity: ['', [Validators.required, Validators.min(0), Validators.max(1_000_000)]],
    });
  }

  onSubmit() {
    const value = this.restockForm.value;
    this.http.post(`${environment.apiBasePath}/items/restock/${value.id}?addQuantity=${value.addQuantity}`, {})
      .subscribe({
        next: () => this.errorService.handleSuccess(`Successfully restocked item ${value.id}`),
        error: (err) => this.errorService.handle(err)
      });
  }

  startShipping() {
    this.http.post(`${environment.apiBasePath}/orders/start-shipping`, {})
      .subscribe({
        next: () => this.errorService.handleSuccess(`Successfully started order shipping job`),
        error: (err) => this.errorService.handle(err)
      });
  }
}
