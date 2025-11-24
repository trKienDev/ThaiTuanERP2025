import { CommonModule } from "@angular/common";
import { Component, inject, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { RouterLink } from "@angular/router";
import { ExpenseWorkflowTemplateApiService } from "../../services/expense-workflow-template.service";
import { ExpenseWorkflowTemplateFacade } from "../../facades/expense-workflow-template.facade";
import { ExpenseWorkflowTemplateDto } from "../../models/expense-workflow-template.model";

@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule, RouterLink],
      templateUrl: './expense-approval-workflow-engine.component.html',
})
export class ExpenseApprovalWorkflowEngineComponent{
      private readonly toastService = inject(ToastService);
      private readonly WfTemplateSer = inject(ExpenseWorkflowTemplateApiService);
      private readonly dialog = inject(MatDialog);
      private readonly approvalWorkflowTemplateFacade = inject(ExpenseWorkflowTemplateFacade);
      wfTemplates$ = this.approvalWorkflowTemplateFacade.approvalWorkflowTemplates$;
      wfTemplates: ExpenseWorkflowTemplateDto[] = [];

      errorMessages:string[] = [];
      
      loading = signal(false);

      trackById(index: number, item: ExpenseWorkflowTemplateDto) { return item.id; }


}
