import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../core/services/api/base-crud.service";
import { CreateTaxRequest, TaxDto, UpdateTaxRequest } from "../../finance/models/tax.dto";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";
import { catchError, forkJoin, Observable, of } from "rxjs";

@Injectable({ providedIn: 'root'})
export class TaxService extends BaseCrudService<TaxDto, CreateTaxRequest, UpdateTaxRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/taxes`);
      }
}