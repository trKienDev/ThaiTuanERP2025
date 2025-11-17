import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { BudgetCodeDto, BudgetCodeRequest } from "../models/budget-code.model";
import { BudgetCodeApiService } from "../services/api/budget-code-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetCodeFacade extends BaseApiFacade<BudgetCodeDto, BudgetCodeRequest> {
      constructor() {
            super(inject(BudgetCodeApiService));
      }
      readonly budgetCodes$: Observable<BudgetCodeDto[]> = this.list$;
}