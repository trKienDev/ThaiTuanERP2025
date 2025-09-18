import { inject, Injectable } from "@angular/core";
import { SupplierService } from "../services/supplier.service";
import { CreateSupplierRequest, SupplierDto, UpdateSupplierRequest } from "../models/supplier.model";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class SupplierFacade extends BaseCrudFacade<SupplierDto, CreateSupplierRequest, UpdateSupplierRequest> {
      constructor() {
            super(inject(SupplierService));
      }
      readonly suppliers$: Observable<SupplierDto[]> = this.list$;
}