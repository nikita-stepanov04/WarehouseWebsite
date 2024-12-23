import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';
import { HomeComponent } from './home/home.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import {httpInterceptorProviders} from './auth/auth-interceptor';
import { AuthTemplateComponent } from './common/auth-template/auth-template.component';
import { FooterComponent } from './common/footer/footer.component';
import { EmailInputComponent } from './common/inputs/email-input/email-input.component';
import { PasswordInputComponent } from './common/inputs/password-input/password-input.component';
import { NameInputComponent } from './common/inputs/name-input/name-input.component';
import { SurnameInputComponent } from './common/inputs/surname-input/surname-input.component';
import { GlobalErrorComponent } from './error/global-error/global-error.component'
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MainTemplateComponent } from './common/main-template/main-template.component';
import { ItemComponent } from './shopping/item/item.component';
import { ItemCategoryPipe } from './shopping/item-category.pipe';
import { PaginationComponent } from './common/pagination/pagination.component';
import { CartComponent } from './shopping/cart/cart.component';
import { CartItemComponent } from './shopping/cart-item/cart-item.component';
import { ModalComponent } from './common/modal/modal.component';
import { ItemDetailedComponent } from './shopping/item-detailed/item-detailed.component';
import { PurchaseComponent } from './shopping/purchase/purchase.component';
import { AddressInputComponent } from './common/inputs/address-input/address-input.component';
import { OrderComponent } from './shopping/order/order.component';
import { AdminOrdersComponent } from './admin/orders/admin-orders.component';
import { MissingComponent } from './admin/missing/missing.component';
import { AdminAddItemComponent } from './admin/add-item/admin-add-item.component';
import { ItemNameComponent } from './common/inputs/item-name/item-name.component';
import { ItemQuantityComponent } from './common/inputs/item-quantity/item-quantity.component';
import { ItemDescriptionComponent } from './common/inputs/item-description/item-description.component';
import { ItemPriceComponent } from './common/inputs/item-price/item-price.component';
import { ItemWeightComponent } from './common/inputs/item-weight/item-weight.component';
import { ItemCategoryComponent } from './common/inputs/item-category/item-category.component';
import { ItemImageComponent } from './common/inputs/item-image/item-image.component';
import { RestockItemComponent } from './admin/restock-item/restock-item.component';
import { IdComponent } from './common/inputs/id/id.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrationComponent,
    HomeComponent,
    AuthTemplateComponent,
    FooterComponent,
    EmailInputComponent,
    PasswordInputComponent,
    NameInputComponent,
    SurnameInputComponent,
    GlobalErrorComponent,
    ModalComponent,
    MainTemplateComponent,
    ItemComponent,
    ItemCategoryPipe,
    PaginationComponent,
    CartComponent,
    CartItemComponent,
    ModalComponent,
    ItemDetailedComponent,
    PurchaseComponent,
    AddressInputComponent,
    OrderComponent,
    AdminOrdersComponent,
    MissingComponent,
    AdminAddItemComponent,
    ItemNameComponent,
    ItemQuantityComponent,
    ItemDescriptionComponent,
    ItemPriceComponent,
    ItemWeightComponent,
    ItemCategoryComponent,
    ItemImageComponent,
    RestockItemComponent,
    IdComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
  ],
  providers: [
    httpInterceptorProviders,
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
