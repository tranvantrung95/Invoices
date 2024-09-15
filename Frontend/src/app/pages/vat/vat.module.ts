import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VatRoutingModule } from './vat-routing.module';
import { VatListComponent } from './vat-list/vat-list.component';
import { VatAddComponent } from './vat-add/vat-add.component';
import { VatEditComponent } from './vat-edit/vat-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzDividerModule } from 'ng-zorro-antd/divider';// For displaying notifications
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';


@NgModule({
  declarations: [
    VatListComponent,
    VatAddComponent,
    VatEditComponent
  ],
  imports: [
    CommonModule,
    VatRoutingModule,
    ReactiveFormsModule,
    NzTableModule,
    NzIconModule,
    NzSpaceModule,
    NzDividerModule,
    NzPageHeaderModule,
    NzButtonModule,
    NzInputModule,
    NzFormModule,
    NzNotificationModule,
    NzMessageModule,
    NzModalModule

  ]
})
export class VatModule { }
