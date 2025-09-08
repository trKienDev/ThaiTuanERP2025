import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { BudgetPlanDto, CreateBudgetPlanRequest, UpdateBudgetPlanRequest } from "../models/budget-plan.model";

@Injectable({ providedIn: 'root' })
export class BudgetPlanService extends BaseCrudService<BudgetPlanDto, CreateBudgetPlanRequest, UpdateBudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }
}