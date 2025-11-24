import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: 'root' })
export class ExpenseStepInstanceApiService {
      private readonly API_URL = `${environment.apiUrl}/workflow/steps`;

      constructor(private readonly http: HttpClient) {}

      
}