import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
      selector: 'outgoing-payment',
      standalone: true,
      templateUrl: './outgoing-payment.component.html',
      imports: [ CommonModule, RouterLink ],
})
export class ExpenseOutgoingPaymentComponent {
      
}