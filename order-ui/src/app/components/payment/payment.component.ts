import { Component, OnDestroy, OnInit } from "@angular/core";
import { PaymentsService } from "../../services/call-api/payments.service";
import { Subject, takeUntil } from "rxjs";

@Component({
  selector: 'app-payment',
  standalone: false,
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit, OnDestroy {
  paymentList: any[] = [];
  isLoading = false;

  private destroy$ = new Subject<void>();

  constructor(
    private paymentsService: PaymentsService,
  ) { }

  ngOnInit(): void {
    this.getPayment();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getPayment(): void {
    this.isLoading = true;
    this.paymentsService.getPayments().pipe(takeUntil(this.destroy$)).subscribe(
      (res: any) => {
        this.paymentList = res;
        this.isLoading = false;
      },
      (error) => {
        this.isLoading = false;
      }
    )
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Succeeded':
        return 'green';
      case 'Pending':
        return 'blue';
      case 'Failed':
        return 'red';
      default:
        return 'default';
    }
  }
}