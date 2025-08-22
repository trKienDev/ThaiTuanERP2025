import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

interface LedgerAccountOption {
      id: string; 
      code: string;
      name: string;
}

@Component({
      selector: 'finance-tax',
      standalone: true,
      imports: [ CommonModule, FormsModule, ReactiveFormsModule ],
      templateUrl: './finance-tax.component.html',
      styleUrl: './finance-tax.component.scss'
})
export class FinanceTaxComponent {

}