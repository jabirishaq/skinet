import { Component } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { OnInit } from '@angular/core';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  products: IProduct[];

  constructor(private shopService: ShopService){
    this.products = [];
  }

  ngOnInit(){
    this.shopService.getProducts().subscribe(response => {
      this.products = response.data;
    }, error => {
      console.log(error);
    })
  }

}
