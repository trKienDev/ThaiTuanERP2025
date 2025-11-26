import { computed, inject, Injectable, signal, WritableSignal } from "@angular/core";
import { ExpensePaymentSummaryDto } from "../models/expense-payment.model";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentApiService } from "../services/api/expense-payment.service";

@Injectable({ providedIn: 'root' })
export class FollowingExpensePaymentFacade {
      private service = inject(ExpensePaymentApiService);

      // ===== State =====
      private items: WritableSignal<ExpensePaymentSummaryDto[]> = signal<ExpensePaymentSummaryDto[]>([]);
      private loading = signal(false);
      private endReached = signal(false);

      // paging
      private page = 1;
      private pageSize = 20;

      // incremental sync mốc thời gian (ISO). Mặc định null cho lần đầu.
      private lastSyncAt: string | null = null;

      // ===== Public API =====
      async loadFirstPage(): Promise<void> {
            // reset state
            this.page = 1;
            this.endReached.set(false);
            this.items.set([]);

            await this.loadPage(this.page);
            // cập nhật mốc sync sau lần đầu lấy dữ liệu (dùng cho incremental refresh)
            this.lastSyncAt = new Date().toISOString();
      }

      async loadNextPage(): Promise<void> {
            if (this.loading() || this.endReached()) return;
            this.page += 1;
            await this.loadPage(this.page);
      }

      /**
       * Refresh incremental: chỉ lấy các bản ghi thay đổi sau lastSyncAt
       * rồi merge (thay thế theo id). Không reset paging.
       * Có thể gọi định kỳ hoặc khi user bấm nút Refresh.
       */
      async refreshIncremental(): Promise<void> {
            console.log('lastSyncAt:', this.lastSyncAt);
            if (!this.lastSyncAt) {
                  // nếu chưa có mốc sync thì fallback loadFirstPage
                  return this.loadFirstPage();
            }

            this.loading.set(true);
            try {
                  const newOnes = await firstValueFrom(
                        this.service.getFollowingPaymentsPaged(1, this.pageSize, this.lastSyncAt)
                  );
                  if (newOnes.length > 0) {
                        const map = new Map(this.items().map(x => [x.id, x]));
                        for (const dto of newOnes) {
                              map.set(dto.id, dto); // replace or insert
                        }
                        // Gộp và giữ thứ tự mới nhất ở trên (CreatedDate giảm dần)
                        const merged = Array.from(map.values()).sort((a, b) =>
                              new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
                        );
                        this.items.set(merged);
                  }
                  this.lastSyncAt = new Date().toISOString();
            } finally {
                  this.loading.set(false);
            }
      }

      // ===== Selectors =====
      readonly list$ = computed(() => this.items());
      readonly loading$ = computed(() => this.loading());
      readonly endReached$ = computed(() => this.endReached());

      // ===== Internal =====
      private async loadPage(page: number): Promise<void> {
            this.loading.set(true);
            try {
                  const data = await firstValueFrom(
                        this.service.getFollowingPaymentsPaged(page, this.pageSize)
                  );
                  if (!data || data.length === 0) {
                        this.endReached.set(true);
                        return;
                  }
                  // append và giữ thứ tự theo CreatedDate desc
                  const merged = [...this.items(), ...data].sort((a, b) =>
                        new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
                  );
                  // loại trùng id nếu server trả lại chồng trang
                  const seen = new Set<string>();
                  const dedup = merged.filter(x => {
                        if (seen.has(x.id)) return false;
                        seen.add(x.id);
                        return true;
                  });
                  this.items.set(dedup);
            } finally {
                  this.loading.set(false);
            }
      }
}

