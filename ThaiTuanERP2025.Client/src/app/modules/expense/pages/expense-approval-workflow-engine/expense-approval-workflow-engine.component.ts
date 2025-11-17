import { CommonModule } from "@angular/common";
import { Component, inject, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { RouterLink } from "@angular/router";
import { ApprovalWorkflowTemplateApiService } from "../../services/approval-workflow-template.service";
import { ApprovalWorkflowTemplateDto } from "../../models/approval-workflow-template.model";
import { ApprovalWorkflowTemplateFacade } from "../../facades/approval-workflow-template.facade";

@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule, RouterLink],
      templateUrl: './expense-approval-workflow-engine.component.html',
})
export class ExpenseApprovalWorkflowEngineComponent{
      private readonly toastService = inject(ToastService);
      private readonly WfTemplateSer = inject(ApprovalWorkflowTemplateApiService);
      private readonly dialog = inject(MatDialog);
      private readonly approvalWorkflowTemplateFacade = inject(ApprovalWorkflowTemplateFacade);
      wfTemplates$ = this.approvalWorkflowTemplateFacade.approvalWorkflowTemplates$;
      wfTemplates: ApprovalWorkflowTemplateDto[] = [];

      errorMessages:string[] = [];
      
      loading = signal(false);

      trackById(index: number, item: ApprovalWorkflowTemplateDto) { return item.id; }


}
