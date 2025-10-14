import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
      selector: 'expense-payment-request-panel',
      standalone: true,
      templateUrl: './expense-payment-request.component.html',
      imports: [CommonModule, RouterLink]
})
export class ExpensePaymentRequestPanelComponent {

}