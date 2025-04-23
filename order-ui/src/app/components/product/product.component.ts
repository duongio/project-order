import { Component } from "@angular/core";
import { Product } from "../../models/data-type";
import { products } from "../../services/data";
import { Router } from "@angular/router";

@Component({
  selector: 'app-product',
  standalone: false,
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent {
  products: Product[] = products;
  showDetail: boolean = false;
  productDetail: Product | undefined;

  constructor(
    private router: Router,
  ) { }

  goToDetail(product: Product) {
    this.showDetail = true;
    this.productDetail = product;
  }

  goToOrder(product: any) {
    this.router.navigate(['/order'], {
      state: { data: product }
    });
  }

  getDataEmitter(data: boolean) {
    this.showDetail = data;
  }
}