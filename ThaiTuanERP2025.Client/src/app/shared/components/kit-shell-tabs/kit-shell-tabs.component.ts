import {
  Component, Input, Output, EventEmitter, Type,
  ChangeDetectionStrategy, OnInit, OnDestroy, inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { loadDomAnimations } from '../../animations/load-dom/load-dom.animation';

export interface KitShellTabDef {
      id: string;              // ví dụ: 'code' | 'group' | 'plan' | 'period'
      label: string;           // nhãn hiển thị
      icon?: string;           // tên icon Material Symbols (tùy chọn)
      component: Type<unknown>;// component panel để render
      hidden?: boolean;     // ẩn tab này (mặc định false)
}

@Component({
      selector: 'kit-shell-tabs',
      standalone: true,
      imports: [CommonModule, RouterOutlet],
      templateUrl: './kit-shell-tabs.component.html',
      styleUrl: './kit-shell-tabs.component.scss',
      animations: [ loadDomAnimations], 
})
export class KitShellTabsComponent implements OnInit, OnDestroy {
      @Input({ required: true }) tabs: KitShellTabDef[] = [];
      @Input() queryParamKey = 'view';
      @Input() sidebarWidth?: number | null | undefined; // px
      @Output() tabChange = new EventEmitter<string>();
      private forceShowHiddenTabId?: string;

      selectedId!: string;

      private router = inject(Router);
      public route = inject(ActivatedRoute);
      private destroy$ = new Subject<void>();

      ngOnInit() {
            this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe((map) => {
                  const qpId = map.get(this.queryParamKey);
                  const fallback = this.tabs.find(t => !t.hidden)?.id;
                  const next = (qpId && this.tabs.some(t => t.id === qpId)) ? qpId : fallback;

                  if (!next) return;
                  if (next !== qpId) {
                        const merged = { ...this.route.snapshot.queryParams, [this.queryParamKey]: next };
                        this.router.navigate([], { relativeTo: this.route, queryParams: merged, queryParamsHandling: 'merge' });
                  }

                  if (next !== this.selectedId) {
                        this.selectedId = next;
                        this.tabChange.emit(next);

                        // 👇 Nếu tab được chọn là tab ẩn, và có flag "cho phép 1 lần" → mở khóa nút trên sidebar
                        const selectedTab = this.tabs.find(t => t.id === next);
                        if (selectedTab?.hidden) {
                              const allowOnce = sessionStorage.getItem('allowPaymentDetailOnce') === '1';
                              if (allowOnce) {
                                    this.forceShowHiddenTabId = next;             // mở khóa hiển thị nút tab
                                    sessionStorage.removeItem('allowPaymentDetailOnce'); // dùng xong thì xoá flag
                              }
                        }
                  }
            });
      }

      selectTab(id: string) {
            if (id === this.selectedId) return;

            // 👇 nếu rời tab đã được mở khoá tạm thời → khoá lại
            if (this.forceShowHiddenTabId && id !== this.forceShowHiddenTabId) {
                  this.forceShowHiddenTabId = undefined;
            }

            this.selectedId = id;
            this.tabChange.emit(id);
            const merged = { ...this.route.snapshot.queryParams, [this.queryParamKey]: id };
            this.router.navigate([], { relativeTo: this.route, queryParams: merged, queryParamsHandling: 'merge' });
      }


      ngOnDestroy() {
            this.destroy$.next();
            this.destroy$.complete();
      }

      public get displayTabs(): KitShellTabDef[] {
            return this.tabs.filter(t =>
                  !t.hidden ||
                  t.id === this.selectedId ||
                  (this.forceShowHiddenTabId && t.id === this.forceShowHiddenTabId)
            );
      }

      
}
