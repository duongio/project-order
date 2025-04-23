import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Product } from "../../../models/data-type";
import { Router } from "@angular/router";

@Component({
  selector: 'app-product-detail',
  standalone: false,
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent {
  @Input() productDetail: Product | undefined;
  @Output() dataEmitter = new EventEmitter<boolean>();

  constructor(
    private router: Router,
  ) { }

  goBack() {
    this.dataEmitter.emit(false);
  }

  goToOrder(product: any) {
    this.router.navigate(['/order'], {
      state: { data: product }
    });
  }
}