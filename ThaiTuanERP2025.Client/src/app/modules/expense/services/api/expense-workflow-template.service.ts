import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { BaseApiService } from "../../../../shared/services/base-api.service";
import { ExpenseWorkflowTemplateDto, ExpenseWorkflowTemplatePayload } from "../../models/expense-workflow-template.model";

@Injectable({ providedIn: 'root' })
export class ExpenseWorkflowTemplateApiService extends BaseApiService<ExpenseWorkflowTemplateDto, ExpenseWorkflowTemplatePayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/expense-workflow-template`);
      }
}