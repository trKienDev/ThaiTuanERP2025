import { Injectable } from "@angular/core";
import { BudgetGroupDto, BudgetGroupRequest } from "../../models/budget-group.model";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class BudgetGroupApiService extends BaseApiService<BudgetGroupDto, BudgetGroupRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/budget-group`);
      }

}