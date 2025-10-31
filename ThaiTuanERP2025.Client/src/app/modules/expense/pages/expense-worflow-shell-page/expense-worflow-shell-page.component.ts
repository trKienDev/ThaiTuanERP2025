import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { ListExpenseWorkflowsPanelComponent } from "./list-expense-workflows/list-expense-workflows.component";

@Component({
      selector: 'expense-workflow-shell-page',
      templateUrl: './expense-worflow-shell-page.component.html',
      imports: [ CommonModule ],
      standalone: true  
})
export class ExpenseWorkflowShellPageComponent {
      readonly tabs = [
            { id: 'expense-payments', label: 'Luồng duyệt', component: ListExpenseWorkflowsPanelComponent }
      ]
}