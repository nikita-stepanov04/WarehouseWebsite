import {Component} from '@angular/core';
import {TokenStorageService} from '../../auth/token-storage.service';
import {CartService} from '../../shopping/cart.service';

@Component({
  selector: 'app-main-template',
  templateUrl: './main-template.component.html'
})
export class MainTemplateComponent {
  constructor(
    public tokenService: TokenStorageService,
    public cartService: CartService) {}

}
