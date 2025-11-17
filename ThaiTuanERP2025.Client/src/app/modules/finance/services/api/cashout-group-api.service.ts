import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../../environments/environment";
import { CashoutGroupDto, CashoutGroupRequest } from "../../models/cashout-group.model";
import { BaseApiService } from "../../../../shared/services/base-api.service";

@Injectable({ providedIn: 'root' })
export class CashoutGroupApiService extends BaseApiService<CashoutGroupDto, CashoutGroupRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/cashout-groups`);
      }
}