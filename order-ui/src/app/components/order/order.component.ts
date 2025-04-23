import { Component, OnInit } from "@angular/core";
import { Product } from "../../models/data-type";
import { Router } from "@angular/router";
import { OrdersService } from "../../services/call-api/orders.service";
import { customerId } from "../../constants/data-constant";
import { NzModalService } from 'ng-zorro-antd/modal';
import { PINComponent } from "./PIN/PIN.component";
import { NzMessageService } from 'ng-zorro-antd/message';
import { Subject, takeUntil } from "rxjs";

@Component({
  selector: 'app-order',
  standalone: false,
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {
  customerId: number = customerId;
  product: Product | undefined;
  isLoading: boolean = false;

  private destroy$ = new Subject<void>();

  constructor(
    private router: Router,
    private orderService: OrdersService,
    private nzModal: NzModalService,
    private message: NzMessageService,
  ) { }

  ngOnInit() {
    this.product = history.state.data as Product;
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  popupConfirm(res: boolean) {
    this.nzModal.confirm({
      nzTitle: 'Xác nhận đơn hàng',
      nzContent: res ? 'Đặt hàng thành công' : 'Đặt hàng thất bại',
      nzOkText: 'OK',
      nzCancelText: 'Hủy',
      nzClassName: 'custom-modal',
      nzOnOk: () => {
        this.router.navigate(['/']);
      },
      nzOnCancel: () => {
        this.router.navigate(['/']);
      }
    });
  }

  onPIN() {
    const modal = this.nzModal.create({
      nzTitle: 'Xác thực mã PIN',
      nzContent: PINComponent,
      nzFooter: null,
    });

    modal.afterClose.subscribe((pin: string) => {
      if (pin && /^\d+$/.test(pin)) {
        const pinNumber = parseInt(pin, 10);
        const data = {
          customerId: this.customerId,
          items: [
            {
              productId: this.product?.id,
              quantity: 1,
              unitPrice: this.product?.price,
            },
          ],
          PIN: pinNumber,
        };
        this.onOrder(data);
      }
      else {
        this.message.error('Mã PIN không hợp lệ!', { nzDuration: 2000 });
      }
    });
  }

  onOrder(data: any) {
    this.isLoading = true;
    this.orderService.postOrder(data).pipe(takeUntil(this.destroy$)).subscribe(
      response => {
        this.isLoading = false;
        this.popupConfirm(true);
      },
      error => {
        this.isLoading = false;
        this.popupConfirm(false);
      }
    );
  }
}