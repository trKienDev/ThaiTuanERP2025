import { BaseApiService } from "../../../shared/services/base-api.service";
import { CommentDetailDto, CommentDto, CommentPayload } from "../models/comment.model";
import { environment } from "../../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApiResponse } from "../../../shared/models/api-response.model";
import { handleApiResponse$ } from "../../../shared/operators/handle-api-response.operator";

@Injectable({ providedIn: 'root' })
export class CommentApiService extends BaseApiService<CommentPayload, CommentDto, CommentDetailDto> {
      constructor(http: HttpClient) {
            super(http, `${environment.server.apiUrl}/comment`);
      }

      getComments(documentType: string, documentId: string): Observable<CommentDetailDto[]> {
            return this.http.get<ApiResponse<CommentDetailDto[]>>(`${this.endpoint}?documentType=${documentType}&documentId=${documentId}`)
                  .pipe(handleApiResponse$<CommentDetailDto[]>());
      }

      reply(parentId: string, payload: CommentPayload): Observable<CommentDetailDto> {
            return this.http.post<ApiResponse<CommentDetailDto>>(`${this.endpoint}/${parentId}/reply`, payload)
                  .pipe(handleApiResponse$<CommentDetailDto>());
      } 
}