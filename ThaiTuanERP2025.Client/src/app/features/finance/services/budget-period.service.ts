import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPeriodDto, BudgetPeriodRequest } from "../models/budget-period.model";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodService extends BaseCrudService<BudgetPeriodDto, BudgetPeriodRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/budget-period`);
      }
}