import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EntryComponent } from "./entry.component";
import { ProductComponent } from "../components/product/product.component";
import { OrderComponent } from "../components/order/order.component";
import { PaymentComponent } from "../components/payment/payment.component";
import { ReportComponent } from "../components/report/report.component";

const routes: Routes = [
    { path: '', component: EntryComponent },
    { path: 'product', component: ProductComponent },
    { path: 'order', component: OrderComponent },
    { path: 'payment-history', component: PaymentComponent },
    { path: 'report', component: ReportComponent },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EntryRoutingModule { }