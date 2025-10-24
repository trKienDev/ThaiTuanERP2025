import { Component } from "@angular/core";
import { OutgoingBankAccountComponent } from "./outgoing-bank-account/outgoing-bank-account.component";
import { KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { SupplierBankAccountComponent } from "./supplier-bank-account/supplier-bank-account.component";

@Component({
      selector: 'bank-account-shell-page',
      standalone: true,
      templateUrl: './bank-account-shell-page.component.html',
      imports: [KitShellTabsComponent],
}) 
export class BankAccountShellPageComponent {
      readonly tabs = [
            { id: 'outgoing-bank-account', label: 'Tài khoản tiền ra', component: OutgoingBankAccountComponent },
            { id: 'supplier-bank-account', label: 'Nhà cung cấp', component: SupplierBankAccountComponent}, 
      ]
}