import { Routes } from "@angular/router";
import { ExpenseComponent } from "./expense.component";

export const expenseRoutes: Routes = [
      {
            path: '',
            component: ExpenseComponent,
            children: [
                  { path: '', redirectTo: 'supplier', pathMatch: 'full'},
                  { path: 'supplier', loadComponent: () => import('./pages/supplier/supplier.component').then((m) => m.SupplierComponent )},
                  { path: 'partner-bank-account', loadComponent:  () => import('./pages/partner-bank-account/partner-bank-account.component').then((m) => m.PartnerBankAccountComponent )},
            ]
      }
]