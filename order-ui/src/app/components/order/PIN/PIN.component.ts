import { Component } from "@angular/core";
import { NzModalRef } from "ng-zorro-antd/modal";

@Component({
  selector: 'app-pin',
  standalone: false,
  templateUrl: './PIN.component.html',
  styleUrls: ['./PIN.component.scss']
})
export class PINComponent {
  pinValue: string = '';

  constructor(
    private modalRef: NzModalRef,
  ) { }

  onSubmit(): void {
    this.modalRef.close(this.pinValue);
  }

  onCancel(): void {
    this.modalRef.destroy();
  }
}