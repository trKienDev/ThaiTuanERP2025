import { Routes } from "@angular/router";
import { FinanceComponent } from "./finance.component";

export const financeRoutes: Routes = [
      {
            path: '',
            component: FinanceComponent,
            children: [
                  { path: '', redirectTo: 'budget-group', pathMatch: 'full' },
                  { path: 'budget-group', loadComponent: () => import('./pages/budget-group/budget-group.component').then((m) => m.BudgetGroupComponent )},
                  { path: 'budget-code', loadComponent: () => import('./pages/budget-code/budget-code.component').then((m) => m.BudgetCodeComponent)},
                  { path: 'budget-period', loadComponent: () => import('./pages/budget-period/budget-period.component').then((m) => m.BudgetPeriodComponent )},
                  { path: 'budget-plan', loadComponent: () => import('./pages/budget-plan/budget-plan.component').then((m) => m.BudgetPlanComponent )},
                  { path: 'bank-account', loadComponent: () => import('./pages/bank-account/bank-account.component').then((m) => m.BankAccountComponent )},
                  { path: 'tax', loadComponent: () => import('./pages/finance-tax/finance-tax.component').then((m) => m.FinanceTaxComponent )},
                  { path: 'ledger-account', loadComponent: () => import('./pages/ledger-account-shell-page/ledger-account-shell-page.component').then((m) => m.LedgerAccountShellPageComponent)},
                  { path: 'cashout', loadComponent: () => import('./pages/cashout/cashout-shell-page.component').then((m) => m.CashoutShellPageComponent )},
            ]
      }
];