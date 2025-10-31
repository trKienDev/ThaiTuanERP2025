import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { BudgetCodeDto, BudgetCodeRequest } from "../models/budget-code.model";
import { BudgetCodeService } from "../services/budget-code.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class BudgetCodeFacade extends BaseCrudFacade<BudgetCodeDto, BudgetCodeRequest> {
      constructor() {
            super(inject(BudgetCodeService));
      }
      readonly budgetCodes$: Observable<BudgetCodeDto[]> = this.list$;
}