import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";

@Component({
      selector: 'payment-request',
      standalone: true,
      imports: [ CommonModule, MatButtonModule, RouterLink ],
      templateUrl: './payment-request.component.html',
      styleUrl: './payment-request.component.scss',
})
export class ExpensePaymentRequestComponent {
      
}