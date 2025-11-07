import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { BudgetPeriodDto, BudgetPeriodRequest } from "../models/budget-period.model";
import { BudgetPeriodService } from "../services/budget-period.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodFacade extends BaseCrudFacade<BudgetPeriodDto, BudgetPeriodRequest> {
      private readonly budgetPeriodService = inject(BudgetPeriodService);
      constructor() {
            super(inject(BudgetPeriodService));
      }
      
      readonly budgetPeriods$: Observable<BudgetPeriodDto[]> = this.list$;
}