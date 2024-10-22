import { Component } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { BasketItem } from '../../../shared/models/basket';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(public basketService: BasketService){}

  getBasketCount(items: BasketItem[]){
    return items.reduce((sum, item) => sum + item.quantity, 0)
  }
}
