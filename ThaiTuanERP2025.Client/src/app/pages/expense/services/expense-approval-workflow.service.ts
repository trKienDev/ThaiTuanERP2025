import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { CreateExpenseApprovalWorkflowRequest, ExpenseApprovalWorkflowDto } from "../models/expense-approval-workflow.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../core/models/api-response.model";

@Injectable({ providedIn: 'root' })
export class ExpenseApprovalWorkflowService {
      private http = inject(HttpClient);
      private baseUrl = `${environment.apiUrl}/expense/approval-workflows`;

      create(request: CreateExpenseApprovalWorkflowRequest): Observable<ApiResponse<ExpenseApprovalWorkflowDto>> {
            return this.http.post<ApiResponse<ExpenseApprovalWorkflowDto>>(this.baseUrl, request);
      }
}