import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SignupRoutingModule } from './signup-routing.module';
import { SignupComponent } from './signup.component';
import { NzFormModule } from 'ng-zorro-antd/form';
import { ReactiveFormsModule } from '@angular/forms';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';


@NgModule({
  declarations: [
    SignupComponent,

  ],
  imports: [
    CommonModule,
    SignupRoutingModule,
    NzFormModule,
    ReactiveFormsModule,
    NzSelectModule,
    NzButtonModule,
    NzInputModule,
    NzCardModule,
    NzGridModule,
    NzCheckboxModule
  ]
})
export class SignupModule { }
