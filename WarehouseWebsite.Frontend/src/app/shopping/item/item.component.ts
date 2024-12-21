import {Component, Input} from '@angular/core';
import {Item} from '../item';
import {CartService} from '../cart.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html'
})
export class ItemComponent {
  @Input() item: Item | null = null;
  @Input() isCartItem: boolean = false;
  currentUrl: string = '';

  constructor(
    public cartService: CartService,
    private router: Router)  {
    this.currentUrl = router.url;
  }
}
