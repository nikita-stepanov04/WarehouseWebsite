import {Component, Input} from '@angular/core';
import {Item} from '../item';
import {CartService} from '../cart.service';
import {Router} from '@angular/router';
import {TokenStorageService} from '../../auth/token-storage.service';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {ErrorService} from '../../error/error.service';
import {ModalService} from '../../common/modal/modal.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html'
})
export class ItemComponent {
  @Input() item: Item | null = null;
  @Input() isCartItem: boolean = false;
  @Input() isMissing: boolean = false;
  currentUrl: string = '';

  constructor(
    public cartService: CartService,
    private router: Router,
    private errorService: ErrorService,
    private modalService: ModalService,
    private http: HttpClient,
    public tokenService: TokenStorageService)  {
    this.currentUrl = router.url;
  }

  removeItem(itemId: string) {
    this.modalService.openModal('Confirm item removal?')
      .subscribe(result => {
        if (result) {
          this.http.delete(`${environment.apiBasePath}/items/${itemId}`)
            .subscribe({
              next: () => {
                this.errorService.handleSuccess('Successfully started item removing job');
                this.item = null;
              },
              error: (err) => this.errorService.handle(err)
            })
        }
      })
  }
}
