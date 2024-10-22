import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { Brand } from '../../shared/models/brands';
import { Type } from '../../shared/models/type';
import { StoreParams } from '../../shared/models/storeParams';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  constructor(private httpClient: HttpClient) { }

  baseUrl = 'http://localhost:9010/';

  getProductsById(id: string): Observable<Product>
  {
    return this.httpClient.get<Product>(this.baseUrl + 'Catalog/GetProductById/'+id);
  }
  getProducts(storeParams: StoreParams){

    let params = new HttpParams();

    if(storeParams.brandId)
    {
      params = params.append('brandId', storeParams.brandId);
      
    }

    if(storeParams.typeId)
    {
      params = params.append('typeId', storeParams.typeId);
        
    }

    if(storeParams.search){
      params = params.append('search', storeParams.search)
    }

    params = params.append('sort', storeParams.sort);
    params = params.append('pageIndex', storeParams.pageNumer);
    params = params.append('pageSize', storeParams.pageSize);

    return this.httpClient.get<Pagination<Product>>(this.baseUrl+'Catalog/GetAllProducts', {params});
  }

  getBrands()
  {
    return this.httpClient.get<Brand[]>(this.baseUrl+'Catalog/GetAllBrands');
  }

  getTypes()
  {
    return this.httpClient.get<Type[]>(this.baseUrl+'Catalog/GetAllTypes')
  }
}
