import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { BudgetPeriodDto, BudgetPeriodRequest } from "../models/budget-period.model";

@Injectable({ providedIn: 'root' })
export class BudgetPeriodService extends BaseCrudService<BudgetPeriodDto, BudgetPeriodRequest> {
      constructor()
}