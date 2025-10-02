import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LedgerAccountComponent } from './ledger-account/ledger-account.component';
import { LedgerAccountTypeComponent } from './ledger-account-type/ledger-account-type.component';
import { Subject, takeUntil } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

type ViewTab = 'type' | 'account';

@Component({
      selector: 'finance-ledger-account-shell-page',
      standalone: true,
      imports: [CommonModule, LedgerAccountComponent, LedgerAccountTypeComponent],
      templateUrl: './ledger-account-shell-page.component.html',
      styleUrls: ['./ledger-account-shell-page.component.scss'],
})
export class LedgerAccountShellPageComponent implements OnInit, OnDestroy {
      view: ViewTab = 'type';
      private destroy$ = new Subject<void>();

      constructor(private router: Router, private route: ActivatedRoute) {}

      ngOnInit(): void {
            // Chỉ còn điều khiển tab qua query param 'view'
            this.route.queryParamMap
            .pipe(takeUntil(this.destroy$))
            .subscribe(q => {
                  this.view = (q.get('view') as ViewTab) ?? 'type';
            });
      }

      selectView(view: ViewTab) {
            this.view = view;
            this.router.navigate([], {
                  relativeTo: this.route,
                  queryParams: { view },
                  queryParamsHandling: 'merge',
            });
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }
}
