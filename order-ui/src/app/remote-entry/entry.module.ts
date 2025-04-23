import { NgModule } from "@angular/core";
import { EntryComponent } from "./entry.component";
import { EntryRoutingModule } from "./entry-routing.module";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { FormatVNDPipe } from "../pipes/formatVND.pipe";

import { NzIconModule, NZ_ICONS } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzTabsModule } from 'ng-zorro-antd/tabs';

import { ArrowLeftOutline, StarFill } from '@ant-design/icons-angular/icons';

const icons = [
    StarFill,
    ArrowLeftOutline,
]

import { ProductComponent } from "../components/product/product.component";
import { OrderComponent } from "../components/order/order.component";
import { ProductDetailComponent } from "../components/product/product-detail/product-detail.component";
import { PINComponent } from "../components/order/PIN/PIN.component";
import { PaymentComponent } from "../components/payment/payment.component";
import { ReportComponent } from "../components/report/report.component";
import { RevenueComponent } from "../components/report/revenue/revenue.component";
import { OrderStatsComponent } from "../components/report/order-stats/order-stats.component";
import { TopProductsComponent } from "../components/report/top-products/top-products.component";
import { OverviewComponent } from "../components/report/overview/overview.component";

@NgModule({
    declarations: [
        EntryComponent,
        ProductComponent,
        ProductDetailComponent,
        OrderComponent,
        PINComponent,
        PaymentComponent,
        ReportComponent,
        RevenueComponent,
        OrderStatsComponent,
        TopProductsComponent,
        OverviewComponent,

        FormatVNDPipe,
    ],
    imports: [
        CommonModule, FormsModule, ReactiveFormsModule,
        EntryRoutingModule, NzModalModule,

        NzIconModule,
        NzButtonModule,
        NzInputModule,
        NzCardModule,
        NzTableModule,
        NzTagModule,
        NzFormModule,
        NzSelectModule,
        NzDatePickerModule,
        NzTabsModule,
    ],
    providers: [
        { provide: NZ_ICONS, useValue: icons },
    ],
    exports: [
        EntryComponent
    ]
})
export class EntryModule { }