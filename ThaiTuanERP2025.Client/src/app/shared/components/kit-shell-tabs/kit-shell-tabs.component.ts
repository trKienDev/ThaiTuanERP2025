import { Component, Input,  Type, OnInit, OnDestroy} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

export interface KitShellTab {
      id: string;              // ví dụ: 'code' | 'group' | 'plan' | 'period'
      label: string;           // nhãn hiển thị
      icon?: string;           // tên icon Material Symbols (tùy chọn)
      component: Type<unknown>;// component panel để render
      hidden?: boolean;     // ẩn tab này (mặc định false)
      disabled?: boolean; 
}

@Component({
      selector: 'kit-shell-tabs',
      standalone: true,
      imports: [CommonModule, RouterOutlet],
      templateUrl: './kit-shell-tabs.component.html',
      styleUrl: './kit-shell-tabs.component.scss',
      // animations: [ loadDomAnimations], 
})
export class KitShellTabsComponent implements OnInit, OnDestroy {
      @Input() tabs: KitShellTab[] = [];
      /** Bật/tắt cơ chế "mở 1 lần" cho tab ẩn qua sessionStorage */
      @Input() allowOnce = true;
      /** Prefix để tránh đụng key giữa nhiều shell khác nhau */
      @Input() allowOnceStoragePrefix = 'kit-shell-tabs.allowOnce';

      selectedId: string | null = null;

      private readonly destroy$ = new Subject<void>();

      constructor(public route: ActivatedRoute, private readonly router: Router) {}

      // ------- Lifecycle ---------------------------------------------------------

      ngOnInit(): void {
            // 1) Lần đầu: đọc child route hiện tại; nếu không có -> redirect sang tab đầu tiên không hidden
            const initial = this.readChildPath();
            const fallback = this.firstVisibleTabId();
            const next = initial ?? fallback;

            if (!initial && fallback) {
                  // chuyển lần đầu cho sạch URL
                  this.navigateTo(fallback, /*replaceUrl*/ true);
            }
            this.selectedId = next;

            console.log('[KitShellTabs] selectedId:',  this.selectedId,);
            this.router.events.subscribe((e) => {
            if (e instanceof NavigationEnd)
                  console.log('[Router NavigationEnd]',  e.urlAfterRedirects);
            });

            // 2) Theo dõi điều hướng để sync selectedId khi user đổi child-route
            this.router.events
            .pipe(
                  filter((e): e is NavigationEnd => e instanceof NavigationEnd),
                  takeUntil(this.destroy$)
            )
            .subscribe(() => {
                  const current = this.readChildPath();
                  if (current && this.selectedId !== current) {
                  this.selectedId = current;
                  }
            });
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }

      // ------- Template helpers --------------------------------------------------

      trackById = (_: number, t: KitShellTab) => t.id;

      get displayTabs(): KitShellTab[] {
            return this.tabs.filter((t) => {
                  if (!t.hidden) return true;
                  if (t.id === this.selectedId) return true; // đang chọn thì luôn hiển thị

                  if (!this.allowOnce) return false;
                  // Cho phép hiện 1 lần (nếu bên ngoài đã "mở khóa" trước khi điều hướng)
                  return sessionStorage.getItem(this.allowOnceKey(t.id)) === '1';
            });
      }

      // ------- Actions -----------------------------------------------------------

      selectTab(id: string) {
            if (id === this.selectedId) return;
            const tab = this.tabs.find((t) => t.id === id);
            if (!tab || tab.disabled) return;

            this.navigateTo(id);
      }

      // ------- Internals ---------------------------------------------------------

      private readChildPath(): string | null {
            // Lấy segment đầu của child route hiện tại: /parent/<this>
            const child = this.route.firstChild;
            if (!child) return null;

            // Ưu tiên URL thực tế (ổn với case path tham số), fallback path tĩnh
            const seg = child.snapshot.url?.[0]?.path;
            if (seg) return seg;

            const cfgPath = child.snapshot.routeConfig?.path ?? null;
            // Nếu cfgPath là '' (redirect), coi như null
            return cfgPath && cfgPath !== '' ? cfgPath : null;
      }

      private firstVisibleTabId(): string | null {
            const t = this.tabs.find((x) => !x.hidden && !x.disabled);
            return t ? t.id : null;
      }

      private navigateTo(id: string, replaceUrl = false) {
            // Điều hướng sang child route: /parent/<id>
            this.router.navigate([id], {
                  relativeTo: this.route,
                  replaceUrl,
            });

            // "Tiêu thụ" allowOnce nếu có
            sessionStorage.removeItem(this.allowOnceKey(id));

            this.selectedId = id;
      }

      private allowOnceKey(tabId: string) {
            return `${this.allowOnceStoragePrefix}.${tabId}`;
      }

      // ------- Static helper: mở khoá 1 lần từ nơi khác -------------------------

      /** Gọi hàm này TRƯỚC khi navigate tới route cần mở tab ẩn */
      static allowOnce(tabId: string, storagePrefix = 'kit-shell-tabs.allowOnce') {
            sessionStorage.setItem(`${storagePrefix}.${tabId}`, '1');
      }
}
