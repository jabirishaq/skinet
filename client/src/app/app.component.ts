import { RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';  // Import HttpClientModule
import { NgForOf } from '@angular/common'; // Import NgForOf for *ngFor
import { IProduct } from './shared/models/product';
import { IPagination } from './shared/models/pagination';
import { CoreModule } from './core/core.module';
import { Component, OnInit } from '@angular/core';


@Component({
    selector: 'app-root',
    imports: [
        RouterOutlet,
        HttpClientModule, // Add HttpClientModule here
        NgForOf,
        CoreModule
    ],
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  title = 'Skinet';
  products: IProduct[] = [];  // Initialize products as an empty array
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<IPagination>('http://localhost:5126/api/products?pageSize=50').subscribe(
      (response: IPagination) => {

        this.products = response.data;

      },
      error => {
        console.log(error);
      }
    );
  }
}
