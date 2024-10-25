import { Component } from '@angular/core';
import { SharedModule } from '../shared/shared.module';




@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  imports: [SharedModule],
  standalone: true
})
export class HomeComponent {

}
