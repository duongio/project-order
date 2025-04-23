import { Component, Input } from "@angular/core";

@Component({
  selector: 'app-revenue',
  standalone: false,
  templateUrl: './revenue.component.html',
  styleUrls: ['./revenue.component.scss']
})
export class RevenueComponent {
  @Input() revenue: any[] = [];
}