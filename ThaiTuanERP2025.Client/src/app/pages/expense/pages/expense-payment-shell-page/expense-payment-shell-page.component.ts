import { Component } from "@angular/core";
import { KitShellTabDef, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { CommonModule } from "@angular/common";
import { ExpensePaymentRequestPanelComponent } from "./expense-payment-request/expense-payment-request.component";
import { ExpensePaymentsPanelComponent } from "./expense-payments/expense-payments.component";
import { ExpensePaymentDetailPanelComponent } from "./expense-payment-detail/expense-payment-detail.component";

@Component({
      selector: 'expense-payment-shell-page',
      standalone: true,
      templateUrl: './expense-payment-shell-page.component.html',
      imports: [ CommonModule, KitShellTabsComponent]
})
export class ExpensePaymentShellPageComponent {
      readonly tabs: KitShellTabDef[] = [
            { id: 'payments', label: 'Thanh toán của bạn', component: ExpensePaymentsPanelComponent },
            { id: 'payment-request', label: 'Thanh toán mới', component: ExpensePaymentRequestPanelComponent },
            { id: 'payment-detail', label: 'Chi tiết thanh toán', component: ExpensePaymentDetailPanelComponent }
      ];
}