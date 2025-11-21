import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { CashoutCodePanelComponent } from "./cashout-codes-panel/cashout-code-panel.component";
import { CashoutGroupPanelComponent } from "./cashout-groups-panel/cashout-group-panel.component";

@Component({
      selector: 'cashouts-shell-page',
      standalone: true, 
      imports: [CommonModule, KitShellTabsComponent],
      template: `
            <kit-shell-tabs [tabs]="tabs"></kit-shell-tabs>
      `,
})
export class CashoutShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'cashout-codes', label: 'Mã khoản chi', component: CashoutCodePanelComponent },
            { id: 'cashout-groups', label: 'Nhóm khoản chi', component: CashoutGroupPanelComponent },
      ]
}