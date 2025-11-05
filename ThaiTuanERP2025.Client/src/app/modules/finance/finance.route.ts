import { Routes } from "@angular/router";
import { FinanceComponent } from "./finance.component";
import { BudgetCodePanelComponent } from "./pages/budget-shell-page/budget-codes/budget-code.component";
import { BudgetPeriodPanelComponent } from "./pages/budget-shell-page/budget-periods/budget-period.component";
import { BudgetGroupPanelComponent } from "./pages/budget-shell-page/budget-groups/budget-group.component";

export const financeRoutes: Routes = [
      {
            path: '',
            component: FinanceComponent,
            children: [
                  { path: '', redirectTo: 'budgets-shell', pathMatch: 'full' },
                  { path: 'tax', loadComponent: () => import('./pages/finance-tax/finance-tax.component').then((m) => m.FinanceTaxComponent )},
                  { 
                        path: 'budgets-shell', 
                        loadComponent: () => import('./pages/budget-shell-page/budgets-shell-page.component').then((m) => m.BudgetShellPageComponent),
                        children: [
                              { path: '', redirectTo: 'budget-codes', pathMatch: 'full' },
                              { path: 'budget-codes', component: BudgetCodePanelComponent },
                              { path: 'budget-groups', component: BudgetGroupPanelComponent },
                              { path: 'budget-periods', component: BudgetPeriodPanelComponent },
                        ]
                  },
                  { path: 'ledger-account', loadComponent: () => import('./pages/ledger-account-shell-page/ledger-account-shell-page.component').then((m) => m.LedgerAccountShellPageComponent)},                  
                  { path: 'cashouts', loadComponent: () => import('./pages/cashouts-shell-page/cashouts-shell-page.component').then((m) => m.CashoutShellPageComponent )}
            ]
      }
];