import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { BudgetGroupDto, BudgetGroupRequest } from "../models/budget-group.model";
import { BudgetGroupApiService } from "../services/api/budget-group-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class BudgetGroupFacade extends BaseApiFacade<BudgetGroupDto, BudgetGroupRequest> {
      constructor() {
            super(inject(BudgetGroupApiService));
      }
      readonly budgetGroups$: Observable<BudgetGroupDto[]> = this.list$;
}