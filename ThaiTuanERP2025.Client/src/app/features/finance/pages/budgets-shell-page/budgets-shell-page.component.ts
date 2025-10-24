import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { BudgetCodePanelComponent } from "./budget-codes/budget-code.component";
import { BudgetGroupPanelComponent } from "./budget-groups/budget-group.component";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { BudgetPeriodPanelComponent } from "./budget-periods/budget-period.component";

@Component({
      selector: 'budgets-shell-page',
      standalone: true,
      imports: [CommonModule, KitShellTabsComponent],
      templateUrl: './budgets-shell-page.component.html',
})
export class BudgetShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'budget-codes', label: 'Mã ngân sách', component: BudgetCodePanelComponent },
            { id: 'budget-groups', label: 'Nhóm ngân sách', component: BudgetGroupPanelComponent },
            { id: 'budget-periods', label: 'Kỳ ngân sách', component: BudgetPeriodPanelComponent },
      ]
}