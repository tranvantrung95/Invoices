import { InvoiceEditComponent } from './invoice-edit/invoice-edit.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceAddComponent } from './invoice-add/invoice-add.component';

const routes: Routes = [
  { path: 'all', component: InvoiceListComponent },
  { path: 'add', component: InvoiceAddComponent },
  { path: 'edit/:customerinvoiceId', component: InvoiceEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InvoiceRoutingModule {}
