import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css'
})
export class BasketComponent {
  constructor(public basketService: BasketService)
  {}

  removeBasketItem(item: BasketItem)
  {
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: BasketItem)
  {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: BasketItem)
  {
    this.basketService.decrementItemQuantity(item);
  }

  
}
