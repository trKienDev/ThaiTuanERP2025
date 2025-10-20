import { Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { OutgoingPaymentDto, OutgoingPaymentRequest, OutgoingPaymentSummaryDto } from "../models/outgoing-payment.model";
import { OutgoingPaymentService } from "../services/outgoing-payment.service";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class OutgoingPaymentFacade extends BaseCrudFacade<OutgoingPaymentSummaryDto, OutgoingPaymentRequest> {
      constructor(private outgoingPaymentService: OutgoingPaymentService) {
            super(outgoingPaymentService);
      }  
      readonly outgoingPayments$: Observable<OutgoingPaymentSummaryDto[]> = this.list$;
}