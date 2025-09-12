import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { CreateExpenseApprovalWorkflowRequest, ExpenseApprovalWorkflowDto, UpdateExpenseApprovalWorkflowRequest } from "../models/expense-approval-workflow.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";

@Injectable({ providedIn: 'root' })
export class ExpenseApprovalWorkflowService extends BaseCrudService<ExpenseApprovalWorkflowDto, CreateExpenseApprovalWorkflowRequest, UpdateExpenseApprovalWorkflowRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/expense/approval-workflows`);
      }
}