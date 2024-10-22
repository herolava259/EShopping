import { Component, Input } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrl: './product-items.component.css'
})
export class ProductItemsComponent {
  
  @Input() product?: Product;

  constructor(private basketService: BasketService){}

  addItemToBasket(){
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
