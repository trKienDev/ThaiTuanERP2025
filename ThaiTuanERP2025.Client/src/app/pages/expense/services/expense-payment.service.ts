import { Injectable } from "@angular/core";
import { ExpensePaymentDto, ExpensePaymentRequest } from "../models/expense-payment.model";
import { BaseCrudService } from "../../../shared/services/base-crud.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../../environments/environment";

@Injectable({ providedIn: 'root' }) 
export class ExpensePaymentService extends BaseCrudService<ExpensePaymentDto, ExpensePaymentRequest> {
      constructor(http: HttpClient) {
            super(http, `${environment.apiUrl}/expense-payments`);
      }
} 