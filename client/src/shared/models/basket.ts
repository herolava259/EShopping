export interface IBasket {
    userName: string;
    items: BasketItem[];
    totalPrice: number;
}

export interface BasketItem {
    quantity: number;
    imageFile: string;
    price: number;
    productId: string;
    productName: string;
}

export class Basket implements IBasket
{
    userName: string = 'farrer';
    items: BasketItem[] = [];
    totalPrice: number = 0;
    
}

export interface IBasketTotal{
    total: number;
    
}