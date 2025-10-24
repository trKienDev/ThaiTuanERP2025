import { Injectable } from "@angular/core";
import { BaseCrudFacade } from "../../../shared/facades/base-crud.facade";
import { OutgoingPaymentRequest, OutgoingPaymentSummaryDto } from "../models/outgoing-payment.model";
import { OutgoingPaymentService } from "../services/outgoing-payment.service";
import { Observable, shareReplay, switchMap, tap, BehaviorSubject } from "rxjs";

@Injectable({ providedIn: 'root'})
export class OutgoingPaymentFacade extends BaseCrudFacade<OutgoingPaymentSummaryDto, OutgoingPaymentRequest> {
      constructor(private outgoingPaymentService: OutgoingPaymentService) {
            super(outgoingPaymentService);
      }  
      
      readonly outgoingPayments$: Observable<OutgoingPaymentSummaryDto[]> = this.list$;

      /** Stream reload riêng cho danh sách following */
      private readonly followingReload$ = new BehaviorSubject<void>(undefined);

      /** 
       * Danh sách outgoing-payment mà user hiện tại đang follow 
       */
      readonly followingPayments$: Observable<OutgoingPaymentSummaryDto[]> = this.followingReload$.pipe(
            switchMap(() => this.outgoingPaymentService.getFollowing()),
            shareReplay({ bufferSize: 1, refCount: false })
      );

      /** Làm mới danh sách following */
      refreshFollowing(): void {
            this.followingReload$.next();
      }

      /** Override create/update/delete để tự refresh following nếu cần */
      override create(req: OutgoingPaymentRequest) {
            return super.create(req).pipe(tap(() => this.refreshFollowing()));
      }

      override update(id: string, req: OutgoingPaymentRequest) {
            return super.update(id, req).pipe(tap(() => this.refreshFollowing()));
      }
      
      override delete(id: string) {
            return super.delete(id).pipe(tap(() => this.refreshFollowing()));
      }
}