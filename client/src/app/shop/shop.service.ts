import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5126/api';

  constructor(private http: HttpClient) { }

  getProducts(){// getProducts is a objct a generic type
    return this.http.get(this.baseUrl+ 'products');
  }
}
