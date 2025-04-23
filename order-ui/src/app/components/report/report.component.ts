import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { types } from "../../constants/data-constant";
import { ReportsService } from "../../services/call-api/reports.service";

@Component({
  selector: 'app-report',
  standalone: false,
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {
  filterForm!: FormGroup;
  types = types;
  revenue: any[] = [];
  orderStats: any[] = [];
  topProducts: any[] = [];
  overview: any = {};

  constructor(
    private formBuilder: FormBuilder,
    private reportsService: ReportsService,
  ) { }

  ngOnInit(): void {
    this.formInit();
  }

  formInit() {
    this.filterForm = this.formBuilder.group({
      range: [null, Validators.required],
      groupBy: [null, Validators.required]
    })
  }

  onLoc() {
    if (this.filterForm.valid) {
      const { range, groupBy } = this.filterForm.value;

      const from = new Date(range[0]).toISOString();
      const to = new Date(range[1]).toISOString();
      const mappedGroupBy = groupBy === 1 ? 'day' : groupBy === 2 ? 'month' : '';

      const params = {
        From: from,
        To: to,
        GroupBy: mappedGroupBy,
      };

      this.reportsService.getRevenue(params).subscribe(
        (res: any) => {
          this.revenue = res;
        },
        (error) => {
          console.error('Lỗi khi lấy dữ liệu doanh thu', error);
        }
      );
      this.reportsService.getOrderStats(params).subscribe(
        (res: any) => {
          this.orderStats = res;
        },
        (error) => {
          console.error('Lỗi khi lấy dữ liệu số lượng đơn hàng', error);
        }
      );
      this.reportsService.getTopProducts(params).subscribe(
        (res: any) => {
          this.topProducts = res;
        },
        (error) => {
          console.error('Lỗi khi lấy dữ liệu các đơn hàng', error);
        }
      );
      this.reportsService.getOverview(from).subscribe(
        (res: any) => {
          this.overview = res;
        },
        (error) => {
          console.error('Lỗi khi lấy dữ liệu tổng quan', error);
        }
      );
    }
  }
}