import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CashOutGroupDto, CreateCashOutGroupRequest, UpdateCashOutGroupRequest } from "../../finance/models/cash-out-group.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class CashOutGroupService extends BaseCrudService<CashOutGroupDto, CreateCashOutGroupRequest, UpdateCashOutGroupRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-groups`);
      }
}