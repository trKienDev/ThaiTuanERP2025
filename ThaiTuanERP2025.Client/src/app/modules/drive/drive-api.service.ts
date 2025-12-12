import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ApiResponse } from "../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../shared/operators/handle-api-response.operator";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: "root" })
export class DriveApiService {
      private http = inject(HttpClient);
      private API_URL = `${environment.drive.apiUrl}/drive`;

      uploadFile(file: File):Observable<string> {
            const formData = new FormData();
            formData.append("file", file);

            return this.http.post<ApiResponse<string>>(this.API_URL, formData)
                  .pipe(handleApiResponse$<string>());
      }
}