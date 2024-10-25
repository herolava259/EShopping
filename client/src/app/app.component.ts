import { HttpClient } from '@angular/common/http';
import { afterNextRender, Component, Inject, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  title = 'eShopping';

  products: Product[] = [];

  constructor(private basketService: BasketService,){

    afterNextRender(() => {
      const basket_username = localStorage.getItem('basket_username')

      if(basket_username)
      {
        this.basketService.getBasket(basket_username);
      }
    })
  }

  ngOnInit(): void {

    
    
  }
}
