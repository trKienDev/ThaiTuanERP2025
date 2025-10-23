// following-expense-payments.component.ts (refactor)
import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { ExpensePaymentStatusPipe } from '../../../pipes/expense-payment-status.pipe';
import { AvatarUrlPipe } from '../../../../../shared/pipes/avatar-url.pipe';
import { MatDialog } from '@angular/material/dialog';
import { ExpensePaymentDetailDialogComponent } from '../expense-payment-detail/expense-payment-detail-dialog/expense-payment-detail-dialog.component';
import { FollowingExpensePaymentFacade } from '../../../facades/following-expense-payment.facade';
import { KitLoadingSpinnerComponent } from "../../../../../shared/components/kit-loading-spinner/kit-loading-spinner.component";
import { KitRefreshButtonComponent } from "../../../../../shared/components/kit-refresh-button/kit-refresh-button.component";
import { ActivatedRoute } from '@angular/router';

@Component({
      selector: 'expense-payments-panel',
      standalone: true,
      templateUrl: './following-expense-payments.component.html',
      styleUrls: ['./following-expense-payments.component.scss'],
      imports: [CommonModule, ExpensePaymentStatusPipe, AvatarUrlPipe, KitLoadingSpinnerComponent, KitRefreshButtonComponent],
})
export class FollowingExpensePaymentsPanelComponent implements OnInit, OnDestroy {
      private dialog = inject(MatDialog);
      private facade = inject(FollowingExpensePaymentFacade);
      private route = inject(ActivatedRoute);

      trackById = (_: number, item: { id: string }) => item.id;

      paymentsSig = this.facade.list$;         // signal<ExpensePaymentSummaryDto[]>
      loadingSig = this.facade.loading$;       // signal<boolean>
      endReachedSig = this.facade.endReached$; // signal<boolean>

      @ViewChild('infiniteAnchor', { static: true }) infiniteAnchor!: ElementRef<HTMLElement>;
      private observer?: IntersectionObserver;

      async ngOnInit(): Promise<void> {
            await this.facade.loadFirstPage();
            this.setupObserver();

            this.route.queryParamMap.subscribe(params => {
                  const paymentId = params.get('paymentId');
                  if (paymentId) {
                        this.openExpensePaymentDetailDialog(paymentId);
                  }
            });
      }

      ngOnDestroy(): void {
            this.observer?.disconnect();
      }

      openExpensePaymentDetailDialog(paymentId: string) {
            const dialogRef = this.dialog.open(ExpensePaymentDetailDialogComponent, {
                  data: paymentId,
            });

            dialogRef.afterClosed().subscribe((result: any) => {
                  if (result?.success) {
                        this.facade.refreshIncremental();
                  }
            });
      }

      manualRefresh() {
            this.facade.refreshIncremental();
      }

      private setupObserver() {
            // Sentinel-based infinite scroll
            this.observer = new IntersectionObserver(async (entries) => {
                  const entry = entries[0];
                  if (entry.isIntersecting && !this.loadingSig() && !this.endReachedSig()) {
                        await this.facade.loadNextPage();
                  }
            }, {
                  root: null, // viewport
                  rootMargin: '0px',
                  threshold: 1.0, // chạm hẳn vào đáy mới tải
            });

            this.observer.observe(this.infiniteAnchor.nativeElement);
      }
}
