import { inject, Injectable } from "@angular/core";
import { SupplierService } from "../services/supplier.service";
import { SupplierDto, SupplierRequest } from "../models/supplier.model";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class SupplierFacade extends BaseCrudFacade<SupplierDto, SupplierRequest> {
      constructor() {
            super(inject(SupplierService));
      }
      readonly suppliers$: Observable<SupplierDto[]> = this.list$;
}