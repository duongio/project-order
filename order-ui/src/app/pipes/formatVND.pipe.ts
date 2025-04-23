import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "formatVND",
  standalone: false
})
export class FormatVNDPipe implements PipeTransform {
  transform(value: number): string {
    if (!value && value !== 0) return '';
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value);
  }
}
