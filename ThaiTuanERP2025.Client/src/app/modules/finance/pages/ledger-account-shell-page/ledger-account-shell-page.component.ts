import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { LedgerAccountTypePanelComponent } from "./ledger-account-type-panel/ledger-account-type-panel.component";
import { LedgerAccountPanelComponent } from "./ledger-account-panel/ledger-account-panel.component";

@Component({
      selector: 'ledger-account-shell-page',
      standalone: true,
      imports: [ CommonModule, KitShellTabsComponent ],
      template: `
            <kit-shell-tabs [tabs]="tabs"></kit-shell-tabs>
      `
})
export class LedgerAccountShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'ledger-account-types', label: 'Loại tài khoản', component: LedgerAccountTypePanelComponent },
            { id: 'ledger-accounts', label: 'Tài khoản', component: LedgerAccountPanelComponent }

      ]
}