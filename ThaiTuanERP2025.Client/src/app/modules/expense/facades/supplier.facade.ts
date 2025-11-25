import { inject, Injectable } from "@angular/core";
import { SupplierDto, SupplierPayload } from "../models/supplier.model";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { SupplierApiService } from "../services/api/supplier.service";

@Injectable({ providedIn: 'root' })
export class SupplierFacade extends BaseApiFacade<SupplierDto, SupplierPayload, string> {
      constructor() {
            super(inject(SupplierApiService));
      }
      readonly suppliers$: Observable<SupplierDto[]> = this.list$;
}