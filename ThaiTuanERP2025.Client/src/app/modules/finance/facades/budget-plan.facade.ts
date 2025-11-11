import { inject, Injectable } from "@angular/core";
import { BudgetPlanDto, BudgetPlanRequest } from "../models/budget-plan.model";
import { Observable } from "rxjs";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { BudgetPlanService } from "../services/budget-plan.service";

@Injectable({ providedIn: 'root' })
export class BudgetPlanFacade extends BaseCrudFacade<BudgetPlanDto, BudgetPlanRequest> {
      constructor() {
            super(inject(BudgetPlanService));
      }
      readonly budgetPlans$: Observable<BudgetPlanDto[]> = this.list$;
}