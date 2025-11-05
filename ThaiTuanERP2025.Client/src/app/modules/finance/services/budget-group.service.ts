import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetGroupDto, BudgetGroupRequest } from "../models/budget-group.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class BudgetGroupService extends BaseCrudService<BudgetGroupDto, BudgetGroupRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-group`);
      }

}