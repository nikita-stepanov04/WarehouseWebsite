import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from './home/home.component';
import {LoginComponent} from './login/login.component';
import {AuthGuardService} from './auth/auth-guard.service';
import {RegistrationComponent} from './registration/registration.component';
import {CartComponent} from './shopping/cart/cart.component';
import {ItemDetailedComponent} from './shopping/item-detailed/item-detailed.component';
import {PurchaseComponent} from './shopping/purchase/purchase.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegistrationComponent
  },
  {
    path: 'cart',
    component: CartComponent
  },
  {
    path: 'item/:id',
    component: ItemDetailedComponent
  },
  {
    path: 'purchase',
    component: PurchaseComponent,
    canActivate: [AuthGuardService],
    data: {
      expectedRole: 'User'
    }
  },
  { path: '', redirectTo: '/home', pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
