import { ConfirmService } from './../../../../../shared/components/confirm-dialog/confirm.service';
import { BudgetPlanDto } from './../../../models/budget-plan.model';
import { CommonModule } from "@angular/common";
import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { BudgetPlanApproversDialogComponent } from "../../../components/budget-apporver-request-dilaog/budget-approver-request-dialog.component";
import { ListBudgetApproversDialogComponent } from "../../../components/list-budget-approvers-dialog/list-budget-approvers-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { BudgetPlanService } from "../../../services/budget-plan.service";
import { combineLatest, distinctUntilChanged, filter, firstValueFrom, map, shareReplay, startWith, Subject, takeUntil } from 'rxjs';
import { BudgetPeriodFacade } from '../../../facades/budget-period.facade';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BudgetPlanDetailDialogComponent } from '../../../components/budget-plan-detail-dialog/budget-plan-detail-dialog.component';
import { BudgetPlanStatusPipe } from "../../../pipes/budget-plan-status.pipe";
import { handleHttpError } from '../../../../../shared/utils/handle-http-errors.util';

@Component({
      selector: 'budget-plan-panel',
      standalone: true,
      imports: [CommonModule, HasPermissionDirective, ReactiveFormsModule, BudgetPlanStatusPipe],
      templateUrl: './budget-plan.component.html',
})
export class BudgetPlanPanelComponent implements OnInit, OnDestroy {
      private readonly dialog = inject(MatDialog);
      private readonly route = inject(ActivatedRoute);
      private readonly router = inject(Router);
      private readonly budgetPlanService = inject(BudgetPlanService);
      private readonly formBuilder = inject(FormBuilder);
      private readonly matDialog = inject(MatDialog);
      private readonly confirm = inject(ConfirmService);

      budgetPlans: BudgetPlanDto[] = [];

      private readonly destroy$ = new Subject<void>();

      private readonly budgetPeriodFacade = inject(BudgetPeriodFacade);
      budgetPeriods$ = this.budgetPeriodFacade.budgetPeriods$;

      periodForm: FormGroup = this.formBuilder.group({
            year: [null as number | null],
            month: [null as number | null],
      });

      years$ = this.budgetPeriods$.pipe(
            map(periods =>
                  [...new Set(periods.map(p => p.year))].sort((a, b) => b - a)
            ),
            shareReplay({ bufferSize: 1, refCount: true })
      );

      months$ = combineLatest([
            this.budgetPeriods$,
            this.periodForm.get('year')!.valueChanges.pipe(
                  startWith(this.periodForm.get('year')!.value)
            ),
      ]).pipe(
            map(([periods, year]) =>
                  year
                        ? periods
                              .filter(p => p.year === year)
                              .map(p => p.month)
                              .sort((a, b) => a - b)
                        : []
            ),
            shareReplay({ bufferSize: 1, refCount: true })
      );

      selectedBudgetPeriodId$ = combineLatest([
            this.budgetPeriods$,
            this.periodForm.valueChanges.pipe(startWith(this.periodForm.value))
      ]).pipe(
            map(([periods, form]) => {
                  const { year, month } = form as { year: number | null; month: number | null };

                  if (!year || !month) return null;

                  const period = periods.find(p => p.year === year && p.month === month);
                  return period ? period.id : null;
            }),
            distinctUntilChanged(),
            shareReplay({ bufferSize: 1, refCount: true })
      );

      ngOnInit(): void {
            this.initializePeriodSelection();
            this.listenOpenByQueryParam();
      }

      private initializePeriodSelection(): void {
            this.years$
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(years => {
                        if (!years.length) return;

                        const currentYear = new Date().getFullYear();
                        const defaultYear = years.includes(currentYear) ? currentYear : years[0];

                        const yearCtrl = this.periodForm.get('year');
                        if (!yearCtrl?.value) {
                              yearCtrl?.setValue(defaultYear);
                        }
                  });

            // 2. Khi danh sách tháng theo year thay đổi -> auto chọn tháng nhỏ nhất nếu chưa chọn / không hợp lệ
            this.months$
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(months => {
                        const monthCtrl = this.periodForm.get('month');
                        if (!monthCtrl) return;

                        const current = monthCtrl.value as number | null;

                        if (!months.length) {
                              monthCtrl.setValue(null);
                              return;
                        }

                        const currentMonth = new Date().getMonth() + 1;
                        if (months.includes(currentMonth)) {
                              if (current !== currentMonth) {
                                    monthCtrl.setValue(currentMonth);
                              }
                              return;
                        }

                        if (!current || !months.includes(current)) {
                              // ❗ KHÔNG dùng emitEvent: false → để form.valueChanges emit và selectedBudgetPeriodId$ chạy
                              monthCtrl.setValue(months[0]);
                        }
                  });

            // 3. Mỗi khi (year, month) hợp lệ -> lấy budgetPeriodId -> load budget plans
            this.selectedBudgetPeriodId$
                  .pipe(
                        takeUntil(this.destroy$),
                        filter((id): id is string => !!id)
                  )
                  .subscribe(periodId => {
                        this.loadBudgetPlans(periodId);
                  });
      }
      private async loadBudgetPlans(budgetPeriodId: string) {
            this.budgetPlans= await firstValueFrom(this.budgetPlanService.getFollowing(budgetPeriodId));
            console.log('budget plans: ', this.budgetPlans);
      }

      private listenOpenByQueryParam(): void {
            this.route.queryParamMap.subscribe(params => {
                  const planId = params.get('openBudgetPlanId');

                  if (planId) {
                        this.activateBudgetPlanDetailDialog(planId);
                  }
            });
      }
      private async activateBudgetPlanDetailDialog(planId: string): Promise<void> {
            let plan: BudgetPlanDto;
            try {
                  plan = await firstValueFrom(this.budgetPlanService.getById(planId));
            } catch (e) {
                  const messages = handleHttpError(e).join('\n');
                  this.confirm.error$(messages);
                  console.error('Không load được BudgetPlan', e);
                  return;
            }

            console.log('plan: ', plan);
            const dialogRef = this.matDialog.open(BudgetPlanDetailDialogComponent, {
                  data: plan 
            });

            // Clear query params khi đóng dialog (tránh auto-open khi refresh)
            dialogRef.afterClosed().subscribe(() => {
                  this.router.navigate([], {
                        relativeTo: this.route,
                        queryParams: { openBudgetPlanId: null },
                        queryParamsHandling: 'merge'
                  });
            });
      }

      // =============================

      trackById(index: number, item: BudgetPlanDto) { return item.id; }

      openListBudgetApproverDialog() {
            const dialogRef = this.dialog.open(ListBudgetApproversDialogComponent, {})
            dialogRef.afterClosed().subscribe();
      }

      openBudgetPlanApproversDialog() {
            const dialogRef = this.dialog.open(BudgetPlanApproversDialogComponent, {})
            dialogRef.afterClosed().subscribe();
      }

      openBudgetPlanDetailDialog(plan: BudgetPlanDto) {
            const dialogRef = this.dialog.open(BudgetPlanDetailDialogComponent, { data: plan });
            dialogRef.afterClosed().subscribe({
                  
            })
      }

      // ===== Destroy ====
      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }
}
