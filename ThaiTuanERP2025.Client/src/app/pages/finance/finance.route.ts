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
            ]
      }
];