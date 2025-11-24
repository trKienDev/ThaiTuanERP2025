import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { KitShellTabsComponent } from "../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component";
import { ExpenseWorkflowsPanelComponent } from "./expense-workflows-panel/expense-workflows-panel.component";
import { ExpenseWorkflowRequestPanel } from "./expense-workflow-request-panel/expense-workflow-request-panel.component";
import { UpdateExpenseWorkflowPanelComponent } from "./update-expense-workflow-panel/update-expense-workflow-panel.component";

@Component({
      selector: 'expense-workflow-shell-page',
      imports: [CommonModule, KitShellTabsComponent],
      standalone: true, 
      template: `
            <kit-shell-tabs [tabs]="tabs"></kit-shell-tabs>
      `
})
export class ExpenseWorkflowShellPageComponent {
      readonly tabs = [
           { id: 'expense-workflows', label: 'Luồng duyệt', component: ExpenseWorkflowsPanelComponent },
           { id: 'expense-workflow-request', label: 'Tạo luồng duyêt', component: ExpenseWorkflowRequestPanel },
           { id: 'update-expense-workflow', label: 'Sửa luồng duyêt', component: UpdateExpenseWorkflowPanelComponent, hidden: true },
      ]
}