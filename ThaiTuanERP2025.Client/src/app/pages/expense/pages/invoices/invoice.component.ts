import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { RouterLink } from "@angular/router";

@Component({
      selector: 'expense-invoice',
      standalone: true,
      imports: [CommonModule, MatButtonModule, RouterLink],
      templateUrl: './invoice.component.html',
})
export class ExpenseInvoiceComponent {
      
}