import { Injectable } from "@angular/core";
import { OutgoingPaymentPayload, OutgoingPaymentSummaryDto } from "../models/outgoing-payment.model";
import { Observable, shareReplay, switchMap, tap, BehaviorSubject } from "rxjs";
import { BaseApiFacade } from "../../../shared/facades/base-api.facade";
import { OutgoingPaymentApiService } from "../services/api/outgoing-payment.service";

@Injectable({ providedIn: 'root'})
export class OutgoingPaymentFacade extends BaseApiFacade<OutgoingPaymentSummaryDto, OutgoingPaymentPayload> {
      constructor(private outgoingPaymentApiService: OutgoingPaymentApiService) {
            super(outgoingPaymentApiService);
      }  
      
      readonly outgoingPayments$: Observable<OutgoingPaymentSummaryDto[]> = this.list$;

      /** Stream reload riêng cho danh sách following */
      private readonly followingReload$ = new BehaviorSubject<void>(undefined);

      /** 
       * Danh sách outgoing-payment mà user hiện tại đang follow 
       */
      readonly followingPayments$: Observable<OutgoingPaymentSummaryDto[]> = this.followingReload$.pipe(
            switchMap(() => this.outgoingPaymentApiService.getFollowing()),
            shareReplay({ bufferSize: 1, refCount: false })
      );

      /** Làm mới danh sách following */
      refreshFollowing(): void {
            this.followingReload$.next();
      }

      /** Override create/update/delete để tự refresh following nếu cần */
      override create(req: OutgoingPaymentPayload) {
            return super.create(req).pipe(tap(() => this.refreshFollowing()));
      }

      override update(id: string, req: OutgoingPaymentPayload) {
            return super.update(id, req).pipe(tap(() => this.refreshFollowing()));
      }
      
      override delete(id: string) {
            return super.delete(id).pipe(tap(() => this.refreshFollowing()));
      }
}