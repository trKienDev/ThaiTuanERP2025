import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Route, Router } from "@angular/router";
import { Subject, takeUntil } from "rxjs";
import { CashoutGroupComponent } from "./cashout-group/cashout-group.component";
import { CashoutCodeComponent } from "./cashout-code/cashout-code.component";

type ViewTab = 'code' | 'group';

@Component({
      selector: 'finance-cashout-shell-page',
      standalone: true,
      imports: [ CommonModule, CashoutGroupComponent, CashoutCodeComponent ],
      templateUrl: './cashout-shell-page.component.html',
      styleUrl: './cashout-shell-page.component.scss',
})
export class CashoutShellPageComponent implements OnInit, OnDestroy {
      view: ViewTab = 'code';
      private destroy$ = new Subject<void>();

      constructor(private router: Router, private route: ActivatedRoute) {}

      ngOnInit(): void {
            this.route.queryParamMap.pipe(takeUntil(this.destroy$))
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