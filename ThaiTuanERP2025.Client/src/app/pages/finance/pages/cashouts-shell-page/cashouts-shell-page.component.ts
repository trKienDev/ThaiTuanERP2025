import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Subject, takeUntil } from "rxjs";
import { CashoutCodePanelComponent } from "./cashout-codes/cashout-code.component";
import { CashoutGroupPanelComponent } from "./cashout-groups/cashout-group.component";

type ViewTab = 'code' | 'group';

@Component({
      selector: 'cashouts-shell-page',
      standalone: true, 
      imports: [ CommonModule, CashoutCodePanelComponent, CashoutGroupPanelComponent ],
      templateUrl: './cashouts-shell-page.component.html'
})
export class CashoutShellPageComponent implements OnInit, OnDestroy {
      view: ViewTab = 'code';
      private destroy$ = new Subject<void>();

      constructor(private router: Router, private route: ActivatedRoute) {}

      ngOnInit(): void {
            this.route.queryParamMap.pipe(takeUntil(this.destroy$))
                  .subscribe(q => {
                        this.view = (q.get('view') as ViewTab) ?? 'code';
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