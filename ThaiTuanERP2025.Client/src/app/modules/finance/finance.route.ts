import { Routes } from "@angular/router";
import { FinanceComponent } from "./finance.component";
import { BudgetCodePanelComponent } from "./pages/budget-shell-page/budget-codes/budget-code.component";
import { BudgetPeriodPanelComponent } from "./pages/budget-shell-page/budget-periods/budget-period.component";
import { BudgetGroupPanelComponent } from "./pages/budget-shell-page/budget-groups/budget-group.component";
import { BudgetPlanPanelComponent } from "./pages/budget-shell-page/budget-plans/budget-plan.component";
import { BudgetPlanRequestPanelComponent } from "./pages/budget-shell-page/budget-plan-request/budget-plan-request.component";
import { LedgerAccountPanelComponent } from "./pages/ledger-account-shell-page/ledger-account-panel/ledger-account-panel.component";
import { LedgerAccountTypePanelComponent } from "./pages/ledger-account-shell-page/ledger-account-type-panel/ledger-account-type-panel.component";

export const financeRoutes: Routes = [
      {
            path: '',
            component: FinanceComponent,
            children: [
                  { path: '', redirectTo: 'budgets-shell', pathMatch: 'full' },
                  { 
                        path: 'budgets-shell', 
                        loadComponent: () => import('./pages/budget-shell-page/budgets-shell-page.component').then((m) => m.BudgetShellPageComponent),
                        children: [
                              { path: '', redirectTo: 'budget-codes', pathMatch: 'full' },
                              { path: 'budget-codes', component: BudgetCodePanelComponent },
                              { path: 'budget-groups', component: BudgetGroupPanelComponent },
                              { path: 'budget-periods', component: BudgetPeriodPanelComponent },
                              { path: 'budget-plans', component: BudgetPlanPanelComponent },
                              { path: 'budget-plan-request', component: BudgetPlanRequestPanelComponent },
                        ]
                  },
                  { 
                        path: 'ledger-account-shell', 
                        loadComponent: () => import('./pages/ledger-account-shell-page/ledger-account-shell-page.component').then((m) => m.LedgerAccountShellPageComponent),
                        children: [
                              { path: '', redirectTo: 'ledger-accounts', pathMatch: 'full' },
                              { path: 'ledger-accounts', component: LedgerAccountPanelComponent },
                              { path: 'ledger-account-types', component: LedgerAccountTypePanelComponent }
                        ]
                  },                  
                  { path: 'cashouts', loadComponent: () => import('./pages/cashouts-shell-page/cashouts-shell-page.component').then((m) => m.CashoutShellPageComponent )}
            ]
      }
];