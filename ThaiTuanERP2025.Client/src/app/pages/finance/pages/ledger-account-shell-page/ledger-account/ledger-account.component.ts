import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";

@Component({ 
      selector: 'finance-ledger-account',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './ledger-account.component.html',
      styleUrl: './ledger-account.component.scss',
})
export class LedgerAccountComponent {
      @Input() ledgerAccountTypeId!: string;
}