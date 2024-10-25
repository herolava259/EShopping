import { Component, OnInit } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { BasketItem } from '../../../shared/models/basket';
import { AcntService } from '../../account/acnt.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  constructor(public basketService: BasketService, private acntService: AcntService){

  }

  public isUserAuthenticated: boolean = false;

  ngOnInit(): void {
    console.log('current user:');

    this.acntService.currentUser$.subscribe({
      next: (res) => {
        this.isUserAuthenticated = res;
        console.log(`An error occured while setting isUserAuthenticated flag.`);

      }
    })
  }

  getBasketCount(items: BasketItem[])
  {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  public login= () =>{
    this.acntService.login();
  }

  public logout= () =>{
    this.acntService.signout();
  }
}
