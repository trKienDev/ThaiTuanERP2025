import { Injectable } from "@angular/core";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { CreateDivisionRequest, DivisionDto, DivisionSummaryDto, UpdateDivisionRequest } from "../models/division.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class DivisionService extends BaseCrudService<DivisionSummaryDto, CreateDivisionRequest, UpdateDivisionRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/division`)
      }
}