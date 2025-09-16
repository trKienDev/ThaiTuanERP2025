import { CommonModule } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ExpenseApprovalWorkflowService } from "../../services/expense-approval-workflow.service";
import { CreateExpenseApprovalWorkflowRequest, ExpenseApprovalWorkflowDto, WorkflowStepRequest } from "../../models/expense-approval-workflow.model";
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { RouterLink } from "@angular/router";

type StepFlowType = 'any' | 'sequential' | 'all';
interface Step {
      id: string;
      title: string;
      approverIds: string[];
      flowType: StepFlowType;
      slaHours: number | null;
}

@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule, RouterLink],
      templateUrl: './expense-approval-workflow-engine.component.html',
})
export class ExpenseApprovalWorkflowEngineComponent implements OnInit {
      private readonly approvalWorkflowService = inject(ExpenseApprovalWorkflowService);
      private readonly toastService = inject(ToastService);

      errorMessages:string[] = [];
      
      loading = signal(false);
      workflows: ExpenseApprovalWorkflowDto[] = [];

      ngOnInit(): void {
            this.loadExpenseApprovalWorkflows();
      }

      loadExpenseApprovalWorkflows(): void {
            this.approvalWorkflowService.getAll().subscribe({
                  next: (wfs) => {
                        this.workflows = wfs;
                  },
                  error: (err) => {
                        // cách viết chuẩn handleHttpError
                        const messages = handleHttpError(err).join('\n');
                        console.log('message: ', messages);
                        this.toastService.errorRich(messages || 'Tải danh sách workflow thất bại');
                  }
            })
      }


}
