import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserAddComponent } from './user-add/user-add.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzDividerModule } from 'ng-zorro-antd/divider'; // For displaying notifications
import { NzMessageModule } from 'ng-zorro-antd/message';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzModalModule } from 'ng-zorro-antd/modal';

@NgModule({
  declarations: [UserEditComponent, UserListComponent, UserAddComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
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
    NzSelectModule,
    NzSpinModule,
    NzUploadModule,
    NzImageModule,
    NzModalModule
  ],
})
export class UserModule {}
