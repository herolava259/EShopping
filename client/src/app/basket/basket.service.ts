import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Basket, BasketItem, IBasket, IBasketTotal } from '../../shared/models/basket';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = 'http://localhost:9010';
  
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();

  private basketTotal = new BehaviorSubject<IBasketTotal | null>(null);
  basketTotal$ = this.basketTotal.asObservable();


  constructor(private httpClient: HttpClient) 
  { }

  getBasket(username: string)
  {
    return this.httpClient.get<IBasket>(this.baseUrl + '/Basket/GetBasket/farrer').subscribe({
      next: basket=>this.basketSource.next(basket)
    });
  }

  setBasket(basket: IBasket)
  {
    return this.httpClient.post<IBasket>(this.baseUrl + '/Basket/CreateBasket', basket).subscribe({
      next: basket =>{
        this.basketSource.next(basket);
        this.caculateBasketTotal();
      }
    });

  }

  getCurrentBasket()
  {
    return this.basketSource.value;
  }

  addItemToBasket(item: Product, quantity: number = 1)
  {
    const itemToAdd: BasketItem = this.mapProductItemtoBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();

    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

    this.setBasket(basket);
  }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find(x => x.productId == itemToAdd.productId);

    if(item)
    {
      item.quantity += quantity;

    }
    else{
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items
  }

  private createBasket(): Basket{
    const basket = new Basket();
    localStorage.setItem('basket_username', 'farrer'); //TODO: farrer can be replaced with loggedin User
    return basket
  }

  private mapProductItemtoBasketItem(item: Product): BasketItem {
    return {
      productId: item.id,
      productName: item.name,
      price: item.price,
      imageFile: item.imageFile,
      quantity: 0
    }
  }

  private caculateBasketTotal(){
    const basket = this.getCurrentBasket();
    if(!basket) return;

    // we are going to loop over in array cand calculate total

    const total = basket.items.reduce((sums, item) => (item.price * item.quantity) + sums, 0);

    this.basketTotal.next({total});

  }

  incrementItemQuantity(item: BasketItem)
  {
    const basket = this.getCurrentBasket();

    if(!basket) return;

    const foundItemIndex = basket.items.findIndex((x) => x.productId === item.productId);

    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  removeItemFromBasket(item: BasketItem)
  {
    const basket = this.getCurrentBasket();

    if(!basket) return ;

    if(basket.items.some((x) => x.productId === item.productId))
    {
      basket.items = basket.items.filter((x) => x.productId!== item.productId)
      if(basket.items.length>0)
      {
        this.setBasket(basket);
      }
      else{
        this.deleteBasket(basket.userName);
      }
    }
  }

  deleteBasket(userName: string)
  {
    return this.httpClient.delete(this.baseUrl + '/Basket/DeleteBasket/' + userName).subscribe({
      next: (response) =>{
        this.basketSource.next(null);
        this.basketTotal.next(null);
        localStorage.removeItem('basket_username')
      },
      error: (err) => {
        console.log('Error occured while deletion basket');
        console.log(err);
      }
    });
  }

  decrementItemQuantity(item: BasketItem)
  {
    const basket = this.getCurrentBasket();
    if(!basket)
      return ;

    const foundItemIndex = basket.items.findIndex((x) => x.productId === item.productId);

    if(basket.items[foundItemIndex].quantity > 1)
    {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }else{
      this.removeItemFromBasket(item)
    }
  }
}
