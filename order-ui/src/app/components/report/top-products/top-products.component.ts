import { Component, Input } from "@angular/core";

@Component({
  selector: 'app-top-products',
  standalone: false,
  templateUrl: './top-products.component.html',
  styleUrls: ['./top-products.component.scss']
})
export class TopProductsComponent {
  @Input() topProducts: any[] = [];
}