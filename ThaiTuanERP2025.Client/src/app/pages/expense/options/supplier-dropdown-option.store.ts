import { inject, Injectable } from "@angular/core";
import { SupplierFacade } from "../facades/supplier.facade";
import { toDropdownOptions } from "../../../shared/components/kit-dropdown/kit-dropdown-to-options.operator";
import { catchError, of, shareReplay } from "rxjs";

@Injectable({ providedIn: 'root' })
export class SupplierOptionStore {
      private readonly facade = inject(SupplierFacade);

      readonly option$ = this.facade.suppliers$.pipe(
            toDropdownOptions({
                  id: 'id', 
                  label: 'name',
            }),
            catchError(() => of([])),
            shareReplay({ bufferSize: 1, refCount: false })
      )
}