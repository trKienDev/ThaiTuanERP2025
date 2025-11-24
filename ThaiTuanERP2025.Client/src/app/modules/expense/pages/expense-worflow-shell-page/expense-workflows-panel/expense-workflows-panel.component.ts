import { ExpenseWorkflowTemplateDto } from './../../../models/expense-workflow-template.model';
import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { ExpenseWorkflowTemplateApiService } from "../../../services/expense-workflow-template.service";
import { firstValueFrom } from 'rxjs';

@Component({
      selector: 'expense-workflows-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: `expense-workflows-panel.component.html`
})
export class ExpenseWorkflowsPanelComponent implements OnInit {
      private readonly expenseWorkflowTemplateApi = inject(ExpenseWorkflowTemplateApiService);

      ngOnInit(): void {
            this.getAllExpenseWorkflowTemplates();
      }

      trackById(index: number, item: ExpenseWorkflowTemplateDto) { return item.id; }

      public expenseWorkflowTemplates: ExpenseWorkflowTemplateDto[] = [];
      private async getAllExpenseWorkflowTemplates() {
            this.expenseWorkflowTemplates = await firstValueFrom(this.expenseWorkflowTemplateApi.getAll());
            console.log('workflows: ', this.expenseWorkflowTemplates);
      }
}