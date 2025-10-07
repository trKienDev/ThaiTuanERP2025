import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { AddInvoiceLineRequest, AttachInvoiceFileRequest, CreateInvoiceDraftRequest, InvoiceDto, ReplaceMainInvoiceFileRequest } from "../models/invoice.model";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { PagedResult } from "../../../shared/models/paged-result.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class InvoiceService {
      private readonly API_URL = `${environment.apiUrl}/invoices`;

      constructor(private http: HttpClient) {}

      // POST /api/invoices/draft
      createDraft(body: CreateInvoiceDraftRequest): Observable<InvoiceDto> {
            return this.http.post<ApiResponse<InvoiceDto>>(`${this.API_URL}/draft`, body)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }


      // GET /api/invoices/{id}
      getById(id: string): Observable<InvoiceDto> {
            return this.http.get<ApiResponse<InvoiceDto>>(`${this.API_URL}/${id}`)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      getMine(page = 1, pageSize = 20): Observable<PagedResult<InvoiceDto>> {
            const params = new URLSearchParams();
            params.set('page', String(page));
            params.set('pageSize', String(pageSize));

            return this.http.get<ApiResponse<PagedResult<InvoiceDto>>>(`${this.API_URL}/mine?${params.toString()}`)
                  .pipe(handleApiResponse$<PagedResult<InvoiceDto>>());
      }

      // GET /api/invoices?page=&pageSize=&keyword=
      getPaged(page = 1, pageSize = 20, keyword?: string | null): Observable<PagedResult<InvoiceDto>> {
            const params = new URLSearchParams();
            params.set('page', String(page));
            params.set('pageSize', String(pageSize));
            if (keyword) params.set('keyword', keyword);

            return this.http.get<ApiResponse<PagedResult<InvoiceDto>>>(`${this.API_URL}?${params.toString()}`)
                  .pipe(handleApiResponse$<PagedResult<InvoiceDto>>());
      }


      // POST /api/invoices/{id}/lines
      addLine(invoiceId: string, body: AddInvoiceLineRequest): Observable<InvoiceDto> {
            return this.http.post<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/lines`, body)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      // DELETE /api/invoices/{id}/lines/{lineId}
      removeLine(invoiceId: string, lineId: string): Observable<InvoiceDto> {
            return this.http.delete<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/lines/${lineId}`)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      // POST /api/invoices/{id}/files
      attachFile(invoiceId: string, body: AttachInvoiceFileRequest): Observable<InvoiceDto> {
            return this.http.post<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/files`, body)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      // POST /api/invoices/{id}/files/replace-main
      replaceMainFile(invoiceId: string, body: ReplaceMainInvoiceFileRequest): Observable<InvoiceDto> {
            return this.http.post<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/files/replace-main`, body)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      // DELETE /api/invoices/{id}/files/{fileId}
      removeFile(invoiceId: string, fileId: string): Observable<InvoiceDto> {
            return this.http.delete<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/files/${fileId}`)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }

      // POST /api/invoices/{id}/followers  (body là Guid userId)
      addFollower(invoiceId: string, userId: string): Observable<InvoiceDto> {
            return this.http.post<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/followers`, JSON.stringify(userId), {
                  headers: { 'Content-Type': 'application/json' },
            })
            .pipe(handleApiResponse$<InvoiceDto>());
      }

      // DELETE /api/invoices/{id}/followers/{userId}
      removeFollower(invoiceId: string, userId: string): Observable<InvoiceDto> {
            return this.http.delete<ApiResponse<InvoiceDto>>(`${this.API_URL}/${invoiceId}/followers/${userId}`)
                  .pipe(handleApiResponse$<InvoiceDto>());
      }
}