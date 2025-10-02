import { Routes } from "@angular/router";
import { FinanceComponent } from "./finance.component";

export const financeRoutes: Routes = [
      {
            path: '',
            component: FinanceComponent,
            children: [
                  { path: '', redirectTo: 'budget-group', pathMatch: 'full' },
                  { path: 'budget-period', loadComponent: () => import('./pages/budget-period/budget-period.component').then((m) => m.BudgetPeriodComponent )},
                  { path: 'budget-plan', loadComponent: () => import('./pages/budget-plan/budget-plan.component').then((m) => m.BudgetPlanComponent )},
                  { path: 'tax', loadComponent: () => import('./pages/finance-tax/finance-tax.component').then((m) => m.FinanceTaxComponent )},
                  { path: 'budgets', loadComponent: () => import('./pages/budgets-shell-page/budgets-shell-page.component').then((m) => m.BudgetShellPageComponent)},
                  { path: 'ledger-account', loadComponent: () => import('./pages/ledger-account-shell-page/ledger-account-shell-page.component').then((m) => m.LedgerAccountShellPageComponent)},                  
                  { path: 'cashouts', loadComponent: () => import('./pages/cashouts-shell-page/cashouts-shell-page.component').then((m) => m.CashoutShellPageComponent )}
            ]
      }
];