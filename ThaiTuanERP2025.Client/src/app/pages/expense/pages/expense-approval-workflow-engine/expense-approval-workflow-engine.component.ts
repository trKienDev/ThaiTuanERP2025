import { CommonModule } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { RouterLink } from "@angular/router";
import { ApprovalWorkflowTemplateService } from "../../services/approval-workflow-template.service";
import { ApprovalWorkflowTemplateDto } from "../../models/approval-workflow-template.model";


@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule, RouterLink],
      templateUrl: './expense-approval-workflow-engine.component.html',
})
export class ExpenseApprovalWorkflowEngineComponent implements OnInit {
      private readonly toastService = inject(ToastService);
      private readonly WfTemplateSer = inject(ApprovalWorkflowTemplateService);
      wfTemplates: ApprovalWorkflowTemplateDto[] = [];

      loadApprovalWorkflowTemplates(): void {
            this.WfTemplateSer.getAll().subscribe({
                  next: (wfs) => {
                        this.wfTemplates = wfs;
                  },
                  error: 
            })
      }

      errorMessages:string[] = [];
      
      loading = signal(false);




}
