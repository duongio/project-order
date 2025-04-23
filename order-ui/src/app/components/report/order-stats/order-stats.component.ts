import { Component, Input } from "@angular/core";

@Component({
  selector: 'app-order-stats',
  standalone: false,
  templateUrl: './order-stats.component.html',
  styleUrls: ['./order-stats.component.scss']
})
export class OrderStatsComponent {
  @Input() orderStats: any[] = [];
}