import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { CarouselModule } from 'ngx-bootstrap/carousel';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    CarouselModule.forRoot()
  ],
  exports:[
    SharedModule,
    CarouselModule
  ]
})
export class HomeModule { }
