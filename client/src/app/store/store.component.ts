import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StoreService } from './store.service';
import { Product } from '../../shared/models/product';
import { Brand } from '../../shared/models/brands';
import { Type } from '../../shared/models/type';
import { StoreParams } from '../../shared/models/storeParams';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrl: './store.component.css'
})
export class StoreComponent implements OnInit{

  @ViewChild('search')
  searchTerm?: ElementRef;

  products: Product[] = [];

  brands: Brand[] = [];

  types: Type[] = [];

  storeParams: StoreParams ={
    brandId: '',
    typeId: '',
    sort: '',
    pageNumer: 1,
    pageSize: 10,
    search: ''
  };
  totalCount: number = 0;
  sortOptions = [
    { name: 'Alphabetical', value: 'name'},
    { name: 'Price: Ascending', value: 'priceAsc'},
    { name: 'Price: Desccending', value: 'priceDesc'}
  ];

  constructor(private storeService: StoreService) {
    
  }

  ngOnInit(): void {
    this.getProducts();
    this.getTypes();
    this.getBrands();
    
  }

  getProducts()
  {
    this.storeService.getProducts(this.storeParams).subscribe({
      next: response =>{
        this.products = response.data;
        this.storeParams.pageNumer = response.pageIndex;
        this.storeParams.pageSize = response.pageSize;
        this.totalCount = response.count
      },
      error: error => console.log(error)
    })
  }
  getBrands()
  {
    this.storeService.getBrands().subscribe({
      next: response => this.brands = [{id: '', name:'All'}, ...response],
      error: error => console.log(error)
    })
  }
  getTypes()
  {
    this.storeService.getTypes().subscribe({
      next: response =>{
        this.types = [{id:'', name:'All'}, ...response]
      },
      error: error => console.log(error)
    })
  }

  onBrandSelected(brandId: string){
    this.storeParams.brandId = brandId;
    this.getProducts()
  }

  onTypeSelected(typeId: string){
    this.storeParams.typeId = typeId;
    this.getProducts()
  }

  onSortSelected(sort: string)
  {
    this.storeParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any){
    this.storeParams.pageNumer = event.page;
    this.getProducts();
  }

  onSearch()
  {
    this.storeParams.search = this.searchTerm?.nativeElement.value;
    this.storeParams.pageNumer = 1;
    this.getProducts();
  }

  onReset()
  {
    if(this.searchTerm)
    {
      this.searchTerm.nativeElement.value = '';
      this.storeParams = {
        brandId: '',
        typeId: '',
        sort: '',
        pageNumer: 1,
        pageSize: 10,
        search: ''
      }

      this.getProducts();
    }
  }
}
