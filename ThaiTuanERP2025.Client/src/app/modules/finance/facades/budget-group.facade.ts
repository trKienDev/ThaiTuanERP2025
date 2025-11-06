import { inject, Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { BudgetGroupDto, BudgetGroupRequest } from "../models/budget-group.model";
import { BudgetGroupService } from "../services/budget-group.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class BudgetGroupFacade extends BaseCrudFacade<BudgetGroupDto, BudgetGroupRequest> {
      constructor() {
            super(inject(BudgetGroupService));
      }
      readonly budgetGroups$: Observable<BudgetGroupDto[]> = this.list$;
}