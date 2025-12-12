import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { BaseApiService } from "../../../shared/services/base-api.service";
import { ExpenseStepTemplateDto, ExpenseStepTemplatePayload } from "../models/expense-step-template.model";

@Injectable({ providedIn: 'root'})
export class ExpenseStepTemplateApiService extends BaseApiService<ExpenseStepTemplateDto, ExpenseStepTemplatePayload> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/expense-step-templates/`);
      }
}