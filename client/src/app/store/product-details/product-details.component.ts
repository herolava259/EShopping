import { Component, OnInit } from '@angular/core';
import { Product } from '../../../shared/models/product';
import { StoreService } from '../store.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css'
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  quantity= 1;

  constructor(
    private storeService: StoreService, 
    private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService,
    private basketService: BasketService){

    }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct()
  {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if(id)
    {
      this.storeService.getProductsById(id).subscribe({
        next: response =>{
          this.product = response;
          this.bcService.set('productDetails', response.name)
        },
        error: error => console.log(error)
      });
    }
    
  }

  addItemToCart(){

    if(!this.product)
      return ;
    this.basketService.addItemToBasket(this.product, this.quantity)
  }

  incrementQuantity()
  {
    this.quantity++;
  }

  decrementQuantity()
  {
    if(this.quantity==0)
      return;
    this.quantity--;
  }
}
