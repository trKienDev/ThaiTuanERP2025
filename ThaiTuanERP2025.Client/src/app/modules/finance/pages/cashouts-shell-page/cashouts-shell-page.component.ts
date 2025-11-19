import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { CashoutCodePanelComponent } from "./cashout-codes/cashout-code.component";
import { CashoutGroupPanelComponent } from "./cashout-groups/cashout-group.component";

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
            { id: 'code', label: 'Mã dòng tiền ra', component: CashoutCodePanelComponent },
            { id: 'group', label: 'Nhóm dòng tiền ra', component: CashoutGroupPanelComponent },
      ]
}