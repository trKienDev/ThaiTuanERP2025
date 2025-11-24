import { ExpenseWorkflowTemplateDto } from './../../../models/expense-workflow-template.model';
import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { ExpenseWorkflowTemplateApiService } from "../../../services/expense-workflow-template.service";
import { firstValueFrom } from 'rxjs';
import { ActionMenuOption } from '../../../../../shared/components/kit-action-menu/kit-action-menu.model';
import { KitActionMenuComponent } from "../../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { ActivatedRoute, Router } from '@angular/router';
import { KitShellTabsComponent } from '../../../../../shared/components/kit-shell-tabs/kit-shell-tabs.component';

@Component({
      selector: 'expense-workflows-panel',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: `expense-workflows-panel.component.html`
})
export class ExpenseWorkflowsPanelComponent implements OnInit {
      private readonly expenseWorkflowTemplateApi = inject(ExpenseWorkflowTemplateApiService);
      private readonly router = inject(Router);
      private readonly route = inject(ActivatedRoute);

      ngOnInit(): void {
            this.getAllExpenseWorkflowTemplates();
      }

      trackById(index: number, item: ExpenseWorkflowTemplateDto) { return item.id; }

      public expenseWorkflowTemplates: ExpenseWorkflowTemplateDto[] = [];
      private async getAllExpenseWorkflowTemplates() {
            this.expenseWorkflowTemplates = await firstValueFrom(this.expenseWorkflowTemplateApi.getAll());
      }

      buildExpenseWorkflowTemplateActions(index: number): ActionMenuOption[] {
            return [
                  { label: 'Sửa', action: () => this.redirectToUpdateExpenseWorkflowTemplatePanel(index)},
            ]
      }

      redirectToUpdateExpenseWorkflowTemplatePanel(index: number) {
            const workflow = this.expenseWorkflowTemplates[index];
            if (!workflow) return;

            const workflowId = workflow.id;

            KitShellTabsComponent.allowOnce('update-expense-workflow');

            // 2) Điều hướng sang tab sửa
            this.router.navigate(['../update-expense-workflow', workflowId], {
                  relativeTo: this.route
            });
      }     
}