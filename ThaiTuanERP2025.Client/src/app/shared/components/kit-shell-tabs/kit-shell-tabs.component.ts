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

      selectedId!: string;

      private router = inject(Router);
      public route = inject(ActivatedRoute);
      private destroy$ = new Subject<void>();

      ngOnInit() {
            this.route.queryParamMap.pipe(takeUntil(this.destroy$)).subscribe((map) => {
                  const qpId = map.get(this.queryParamKey);
                  const fallback = this.tabs.find(t => !t.hidden)?.id;   // 👈 fallback tab hiển thị
                  const next = (qpId && this.tabs.some(t => t.id === qpId)) ? qpId : fallback;

                  if (!next) return;
                  if (next !== qpId) {
                        const merged = { ...this.route.snapshot.queryParams, [this.queryParamKey]: next };
                        this.router.navigate([], { relativeTo: this.route, queryParams: merged, queryParamsHandling: 'merge' });
                  }

                  if (next !== this.selectedId) {
                        this.selectedId = next;
                        this.tabChange.emit(next);
                  }
            });

            if (!this.selectedId) {
                  this.selectedId = this.tabs.find(t => !t.hidden)?.id ?? this.tabs[0]?.id;
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

      public get displayTabs(): KitShellTabDef[] {
            return this.tabs.filter(t => !t.hidden);
      }
}
