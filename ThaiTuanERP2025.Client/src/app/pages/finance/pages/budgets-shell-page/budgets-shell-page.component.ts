import { CommonModule } from "@angular/common";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subject, takeUntil } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { BudgetCodePanelComponent } from "./budget-codes/budget-code.component";
import { BudgetGroupPanelComponent } from "./budget-groups/budget-group.component";

type ViewTab = 'code' | 'group' | 'plan' | 'period';

@Component({
      selector: 'budgets-shell-page',
      standalone: true,
      imports: [CommonModule, BudgetCodePanelComponent, BudgetGroupPanelComponent ],
      templateUrl: './budgets-shell-page.component.html',
      styleUrl: './budgets-shell-page.component.scss'
})
export class BudgetShellPageComponent implements OnInit, OnDestroy {
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