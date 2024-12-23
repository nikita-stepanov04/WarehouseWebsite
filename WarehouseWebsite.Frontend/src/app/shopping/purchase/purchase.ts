import {OrderLine} from '../order-line';
import {Customer} from './customer';

export class Purchase {
  order: OrderLine[];
  customer: Customer | null;

  constructor(order: OrderLine[], customer: Customer | null) {
    this.order = order;
    this.customer = customer;
  }
}
