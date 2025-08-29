import { Routes } from "@angular/router";
import { ExpenseComponent } from "./expense.component";

export const expenseRoutes: Routes = [
      {
            path: '',
            component: ExpenseComponent,
            children: [
                  { path: '', redirectTo: 'invoice', pathMatch: 'full'},
                  { path: 'invoice', loadComponent: () => import('./pages/invoices/invoice.component').then((m) => m.ExpenseInvoiceComponen )},
                  { path: 'invoice/request', loadComponent: () => import('./pages/invoices/invoice-request/invoice-request-page.component').then((m) => m.InvoiceRequestPageComponent )},
            ]
      }
]