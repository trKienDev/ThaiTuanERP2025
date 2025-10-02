import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";

@Injectable({ providedIn: 'root' })
export class BudgetPlanService extends BaseCrudService<BudgetPlanDto, BudgetPlanRequest>{
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-plan`);
      }
}