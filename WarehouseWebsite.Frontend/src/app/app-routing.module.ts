import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from './home/home.component';
import {LoginComponent} from './login/login.component';
import {AuthGuardService} from './auth/auth-guard.service';
import {RegistrationComponent} from './registration/registration.component';
import {CartComponent} from './shopping/cart/cart.component';
import {ItemDetailedComponent} from './shopping/item-detailed/item-detailed.component';
import {PurchaseComponent} from './shopping/purchase/purchase.component';
import {AdminOrdersComponent} from './admin/orders/admin-orders.component';
import {MissingComponent} from './admin/missing/missing.component';
import {AdminAddItemComponent} from './admin/add-item/admin-add-item.component';
import {RestockItemComponent} from './admin/restock-item/restock-item.component';

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
    path: 'item/:id/:returnUrl',
    component: ItemDetailedComponent
  },
  {
    path: 'purchase',
    component: PurchaseComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'User' }
  },
  {
    path: 'admin-orders/:status',
    component: AdminOrdersComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Admin' }
  },
  {
    path: 'admin-missing',
    component: MissingComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Admin' }
  },
  {
    path: 'admin-add',
    component: AdminAddItemComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Admin' }
  },
  {
    path: 'admin-restock',
    component: RestockItemComponent,
    canActivate: [AuthGuardService],
    data: { expectedRole: 'Admin' }
  },
  { path: '', redirectTo: '/home', pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
