import {
  Component, Input, Output, EventEmitter, Type,
  ChangeDetectionStrategy, OnInit, OnDestroy, inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

export interface KitShellTabDef {
      id: string;              // ví dụ: 'code' | 'group' | 'plan' | 'period'
      label: string;           // nhãn hiển thị
      icon?: string;           // tên icon Material Symbols (tùy chọn)
      component: Type<unknown>;// component panel để render
}

@Component({
      selector: 'kit-shell-tabs',
      standalone: true,
      imports: [CommonModule, RouterOutlet],
      templateUrl: './kit-shell-tabs.component.html',
      styleUrl: './kit-shell-tabs.component.scss',
})
export class KitShellTabsComponent implements OnInit, OnDestroy {
      @Input({ required: true }) tabs: KitShellTabDef[] = [];
      @Input() queryParamKey = 'view';
      @Input() sidebarWidth?: number | null | undefined; // px
      @Output() tabChange = new EventEmitter<string>();

      selectedId!: string;

      private router = inject(Router);
      public route = inject(ActivatedRoute);
      private destroy$ = new Subject<void>();

      ngOnInit() {
            // Lấy từ query param -> nếu không hợp lệ thì rơi về tab đầu
            this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe((map) => {
                  const qpId = map.get(this.queryParamKey);
                  const fallback = this.tabs[0]?.id;
                  const next = (qpId && this.tabs.some(t => t.id === qpId)) ? qpId : fallback;

                  if (!next) return;
                  if (next !== qpId) {
                        // sửa URL nếu query param không hợp lệ
                        const merged = { ...this.route.snapshot.queryParams, [this.queryParamKey]: next };
                        this.router.navigate([], { relativeTo: this.route, queryParams: merged, queryParamsHandling: 'merge' });
                  }

                  if (next !== this.selectedId) {
                        this.selectedId = next;
                        this.tabChange.emit(next);
                  }
            });

            // Fallback sync ngay lần đầu (trường hợp không có subscribe kịp)
            if (!this.selectedId && this.tabs.length) {
                  this.selectedId = this.tabs[0].id;
            }
      }

      selectTab(id: string) {
            if (id === this.selectedId) return;
            this.selectedId = id;
            this.tabChange.emit(id);
            const merged = { ...this.route.snapshot.queryParams, [this.queryParamKey]: id };
            this.router.navigate([], { relativeTo: this.route, queryParams: merged, queryParamsHandling: 'merge' });
      }

      ngOnDestroy() {
            this.destroy$.next();
            this.destroy$.complete();
      }
}
