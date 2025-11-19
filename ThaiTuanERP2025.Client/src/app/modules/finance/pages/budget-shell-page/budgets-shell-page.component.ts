import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { BudgetCodePanelComponent } from "./budget-codes/budget-code.component";
import { KitShellTab, KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { BudgetPeriodPanelComponent } from "./budget-periods/budget-period.component";
import { BudgetGroupPanelComponent } from "./budget-groups/budget-group.component";
import { BudgetPlanPanelComponent } from "./budget-plans/budget-plan.component";
import { BudgetPlanRequestPanelComponent } from "./budget-plan-request/budget-plan-request.component";

@Component({
      selector: 'budgets-shell-page',
      standalone: true,
      imports: [CommonModule, KitShellTabsComponent],
      template: `
            <kit-shell-tabs [tabs]="tabs"></kit-shell-tabs>
      `
})
export class BudgetShellPageComponent {
      readonly tabs: KitShellTab[] = [
            { id: 'budget-codes', label: 'Mã ngân sách', component: BudgetCodePanelComponent },
            { id: 'budget-groups', label: 'Nhóm ngân sách', component: BudgetGroupPanelComponent },
            { id: 'budget-periods', label: 'Kỳ ngân sách', component: BudgetPeriodPanelComponent },
            { id: 'budget-plans', label: 'Kế hoạch ngân sách', component: BudgetPlanPanelComponent },
            { id: 'budget-plan-request', label: 'Tạo kế hoạch ngân sách', component: BudgetPlanRequestPanelComponent }
      ]
}