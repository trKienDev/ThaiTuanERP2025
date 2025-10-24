import { Component } from "@angular/core";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { CommonModule } from "@angular/common";
import { ExpensePaymentRequestPanelComponent } from "./expense-payment-request/expense-payment-request.component";
import { FollowingExpensePaymentsPanelComponent } from "./following-expense-payments/following-expense-payments.component";

@Component({
      selector: 'expense-payment-shell-page',
      standalone: true,
      templateUrl: './expense-payment-shell-page.component.html',
      imports: [ CommonModule, KitShellTabsComponent]
})
export class ExpensePaymentShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'following-payments', label: 'Thanh toán của bạn', component: FollowingExpensePaymentsPanelComponent },
            { id: 'payment-request', label: 'Thanh toán mới', component: ExpensePaymentRequestPanelComponent },
      ];
}