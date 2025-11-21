import { inject, Injectable } from "@angular/core";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { CashoutCodeDto, CashoutCodePayload } from "../models/cashout-code.model";
import { CashoutCodeApiService } from "../services/api/cashout-code-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CashoutCodeFacade extends BaseApiFacade<CashoutCodeDto, CashoutCodePayload> {
      constructor() {
            super(inject(CashoutCodeApiService));
      }
      readonly cashoutGroups$: Observable<CashoutCodeDto[]> = this.list$;
}