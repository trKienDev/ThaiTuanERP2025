import { Injectable } from "@angular/core";
import { environment } from "../../../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class ExpenseStepInstanceApiService {
      private readonly endpoint = `${environment.server.apiUrl}/expense-step-instance`;

      constructor(private readonly http: HttpClient) { }

}