import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ExpenseApprovalWorkflowService } from "../../../services/expense-approval-workflow.service";

@Component({
      selector: 'expense-approval-workflow-engine-request',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './expense-approval-workflow-engine-request.component.html',
      styleUrl: './expense-approval-workflow-engine-request.component.scss',
})
export class ExpenseApprovalWorkflowEngineRequest {
      private readonly approvalWorkflowService = inject(ExpenseApprovalWorkflowService);


}