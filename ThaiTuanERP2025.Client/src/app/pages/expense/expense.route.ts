import { Routes } from "@angular/router";
import { ExpenseComponent } from "./expense.component";

export const expenseRoutes: Routes = [
      {
            path: '',
            component: ExpenseComponent,
            children: [
                  { path: '', redirectTo: 'invoice', pathMatch: 'full'},
                  { path: 'supplier', loadComponent: () => import('./pages/suppliers/supplier.component').then((m) => m.ExpenseSupplierComponent )},
                  { path: 'invoice', loadComponent: () => import('./pages/invoices/invoice.component').then((m) => m.ExpenseInvoiceComponent )},
                  { path: 'invoice/request', loadComponent: () => import('./pages/invoices/invoice-request/invoice-request-page.component').then((m) => m.InvoiceRequestPageComponent )},
                  { path: 'approval-workflow-engine', loadComponent: () => import('./pages/expense-approval-workflow-engine/expense-approval-workflow-engine.component').then((m) => m.ExpenseApprovalWorkflowEngineComponent )},
                  { path: 'payment-request', loadComponent: () => import('./pages/payment-requests/payment-request.component').then((m) => m.ExpensePaymentRequestComponent )},
                  { path: 'payment-request/expense-payment', loadComponent: () => import('./pages/payment-requests/expense-payments/expense-payment.component').then((m) => m.ExpensePaymentComponent )},
            ]
      }
]