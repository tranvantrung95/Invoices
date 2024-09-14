import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/dashboard' },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./pages/dashboard/dashboard.module').then((m) => m.DashboardModule),
  },
  {
    path: 'invoices',
    loadChildren: () =>
      import('./pages/invoice/invoice.module').then((m) => m.InvoiceModule),
  },
  {
    path: 'customers',
    loadChildren: () =>
      import('./pages/customer/customer.module').then((m) => m.CustomerModule),
  },
  {
    path: 'items',
    loadChildren: () =>
      import('./pages/item/item.module').then((m) => m.ItemModule),
  },
  {
    path: 'vat',
    loadChildren: () =>
      import('./pages/vat/vat.module').then((m) => m.VatModule),
  },
  {
    path: 'users',
    loadChildren: () =>
      import('./pages/user/user.module').then((m) => m.UserModule),
  },
  {
    path: 'roles',
    loadChildren: () =>
      import('./pages/roles/roles.module').then((m) => m.RolesModule),
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./pages/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'signup',
    loadChildren: () =>
      import('./pages/signup/signup.module').then((m) => m.SignupModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
