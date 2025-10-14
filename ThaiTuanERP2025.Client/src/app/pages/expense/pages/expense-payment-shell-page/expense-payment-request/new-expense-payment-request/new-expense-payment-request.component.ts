import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { provideMondayFirstDateAdapter } from "../../../../../../shared/date/provide-monday-first-date-adapter";

@Component({
      selector: 'new-expense-payment-request',
      templateUrl: './new-expense-payment-request.component.html',
      imports: [ CommonModule],
      styleUrls: ['./new-expense-payment-request.component.scss'],
      standalone: true,
      providers: [...provideMondayFirstDateAdapter() ]
})
export class NewExpensePaymentRequestComponent {
      
}