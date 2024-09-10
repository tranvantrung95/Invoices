import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VatRoutingModule } from './vat-routing.module';
import { VatListComponent } from './vat-list/vat-list.component';
import { VatAddComponent } from './vat-add/vat-add.component';
import { VatEditComponent } from './vat-edit/vat-edit.component';


@NgModule({
  declarations: [
    VatListComponent,
    VatAddComponent,
    VatEditComponent
  ],
  imports: [
    CommonModule,
    VatRoutingModule
  ]
})
export class VatModule { }
