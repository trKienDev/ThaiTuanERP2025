import { inject, Injectable } from "@angular/core";
import { CashoutGroupDto, CashoutGroupPayload } from "../models/cashout-group.model";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { CashoutGroupApiService } from "../services/api/cashout-group-api.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CashoutGroupFacade extends BaseApiFacade<CashoutGroupDto, CashoutGroupPayload> {
      constructor() {
            super(inject(CashoutGroupApiService));
      }
      readonly cashoutGroups$: Observable<CashoutGroupDto[]> = this.list$;
}