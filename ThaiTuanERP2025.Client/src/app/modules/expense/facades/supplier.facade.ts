import { inject, Injectable } from "@angular/core";
import { SupplierDto, SupplierRequest } from "../models/supplier.model";
import { Observable } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { SupplierApiService } from "../services/supplier.service";

@Injectable({ providedIn: 'root' })
export class SupplierFacade extends BaseApiFacade<SupplierDto, SupplierRequest> {
      constructor() {
            super(inject(SupplierApiService));
      }
      readonly suppliers$: Observable<SupplierDto[]> = this.list$;
}