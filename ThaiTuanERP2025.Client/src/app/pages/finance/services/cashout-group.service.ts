import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { CashoutGroupDto, CreateCashoutGroupRequest, UpdateCashoutGroupRequest } from "../models/cashout-group.model";

@Injectable({ providedIn: 'root' })
export class CashoutGroupService extends BaseCrudService<CashoutGroupDto, CreateCashoutGroupRequest, UpdateCashoutGroupRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-groups`);
      }
}