import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ErrorService} from '../../error/error.service';
import {ModalService} from '../../common/modal/modal.service';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Customer} from './customer';
import {Purchase} from './purchase';
import {CartService} from '../cart.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html'
})
export class PurchaseComponent implements OnInit{
  public purchaseForm: FormGroup;
  private customer: Customer | null = null;

  constructor(
    private fb: FormBuilder,
    private errorService: ErrorService,
    private modalService: ModalService,
    private cartService: CartService,
    private http: HttpClient,
    private router: Router) {

    this.purchaseForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(32)]],
      surname: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(32)]],
      address: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]]
    });
  }

  ngOnInit(): void {
    this.getCustomer();
  }

  getCustomer() {
    this.http.get(`${environment.apiBasePath}/orders/customer-data`)
      .subscribe({
        next: (customer) => {
          this.customer = customer as Customer;
          this.purchaseForm.patchValue({
            email: this.customer.email,
            name: this.customer.name,
            surname: this.customer.surname,
            address: this.customer.address
          })
        },
        error: (err) => this.errorService.handle(err)
      })
  }

  onSubmit() {
    if (this.cartService.cart.length == 0) {
      this.errorService.handleSuccess('Nothing to purchase');
      return;
    }
    this.modalService.openModal('Complete purchase and place order?')
      .subscribe(result => {
        if (result) {
          const formValue = this.purchaseForm.value;
          const formCustomer = new Customer(
            formValue.email,
            formValue.name,
            formValue.surname,
            formValue.address
          );
          const order = new Purchase(this.cartService.cart, null);
          if (!this.compareCustomers(this.customer!, formCustomer)) {
            order.customer = formCustomer;
          }
          this.http.post<Purchase>(`${environment.apiBasePath}/orders/place`, order)
            .subscribe({
              next: () => {
                this.errorService.handleSuccess('Successfully purchased order');
                this.cartService.emptyCart();
                this.router.navigate(['/home']);
              },
              error: (err) => this.errorService.handle(err)
            });
        }
      })
  }

  private compareCustomers(customer1?: Customer, customer2?: Customer): boolean {
    if (!customer1 && !customer2) {
      return true;
    }
    if (!customer1 || !customer2) {
      return false;
    }
    return customer1.email === customer2.email &&
      customer1.name === customer2.name &&
      customer1.surname === customer2.surname &&
      customer1.address === customer2.address;
  }

}
