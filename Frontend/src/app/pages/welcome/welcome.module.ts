import { NgModule } from '@angular/core';

import { WelcomeRoutingModule } from './welcome-routing.module';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';

import { WelcomeComponent } from './welcome.component';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';


@NgModule({
  imports: [WelcomeRoutingModule, NzCardModule, NzGridModule,  NzStatisticModule],// Import Statistic module for KPI display],
  declarations: [WelcomeComponent],
  exports: [WelcomeComponent]
})
export class WelcomeModule { }
