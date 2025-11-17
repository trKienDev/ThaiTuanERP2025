import { inject, Injectable } from "@angular/core";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { BudgetPlanApiService } from "../services/api/budget-plan-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetPlanFacade extends BaseApiFacade<BudgetPlanDto, BudgetPlanRequest> {
      constructor() {
            super(inject(BudgetPlanApiService));
      }
      readonly budgetPlans$: Observable<BudgetPlanDto[]> = this.list$;
}