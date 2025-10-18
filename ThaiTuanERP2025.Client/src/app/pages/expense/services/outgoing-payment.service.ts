import { Injectable } from "@angular/core";
import { OutgoingPaymentDto, OutgoingPaymentRequest } from "../models/outgoing-payment.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class OutgoingPaymentService extends BaseCrudService<OutgoingPaymentDto, OutgoingPaymentRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/outgoing-payments`);
      }
}