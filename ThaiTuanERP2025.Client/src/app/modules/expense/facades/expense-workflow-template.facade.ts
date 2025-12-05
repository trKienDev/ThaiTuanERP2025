import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { ExpenseWorkflowTemplateDto, ExpenseWorkflowTemplatePayload } from "../models/expense-workflow-template.model";
import { ExpenseWorkflowTemplateApiService } from "../services/api/expense-workflow-template.service";

@Injectable({ providedIn: 'root' })
export class ExpenseWorkflowTemplateFacade extends BaseApiFacade<ExpenseWorkflowTemplateDto, ExpenseWorkflowTemplatePayload> {
      constructor() {
            super(inject(ExpenseWorkflowTemplateApiService));
      }
      readonly approvalWorkflowTemplates$: Observable<ExpenseWorkflowTemplateDto[]> = this.list$;
}