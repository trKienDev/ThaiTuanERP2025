import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { BudgetCodePanelComponent } from "./budget-codes/budget-code.component";
import { BudgetGroupPanelComponent } from "./budget-groups/budget-group.component";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";

@Component({
      selector: 'budgets-shell-page',
      standalone: true,
      imports: [CommonModule, KitShellTabsComponent],
      templateUrl: './budgets-shell-page.component.html',
})
export class BudgetShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'code', label: 'Mã ngân sách', component: BudgetCodePanelComponent },
            { id: 'group', label: 'Nhóm ngân sách', component: BudgetGroupPanelComponent }
      ]
}