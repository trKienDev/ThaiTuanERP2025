import { Routes } from "@angular/router";
import { ExpenseComponent } from "./expense.component";
import { paymentDetailEntryGuard } from "./guard/payment-detail-entry.guard";

export const expenseRoutes: Routes = [
      {
            path: '',
            component: ExpenseComponent,
            children: [
                  { path: '', redirectTo: 'invoice', pathMatch: 'full'},
                  { path: 'supplier', data: { animation: 'SupplierPage' }, loadComponent: () => import('./pages/suppliers/supplier.component').then((m) => m.ExpenseSupplierComponent )},
                  { path: 'invoice', data: { animation: 'InvoicePage' }, loadComponent: () => import('./pages/invoices/invoice.component').then((m) => m.ExpenseInvoiceComponent )},
                  { path: 'invoice/request', data: { animation: 'InvoiceRequestPage' }, loadComponent: () => import('./pages/invoices/invoice-request/invoice-request-page.component').then((m) => m.InvoiceRequestPageComponent )},
                  { 
                        path: 'payment-detail/:id', 
                        canActivate: [paymentDetailEntryGuard],
                        data: { animation: 'PaymentDetailPage' },
                        loadComponent: () => import('./pages/expense-payment-detail/expense-payment-detail.component').then((m) => m.ExpensePaymentDetailComponent )
                  },
                  { path: 'expense-payment-shell',  data: { animation: 'ExpensePaymentShellPage' }, loadComponent: () => import('./pages/expense-payment-shell-page/expense-payment-shell-page.component').then((m) => m.ExpensePaymentShellPageComponent), },
                  { path: 'outgoing-payment', loadComponent: () => import('./pages/outgoing-payment/outgoing-payment.component').then((m) => m.ExpenseOutgoingPaymentComponent )},
                  { path: 'outgoing-payment/outgoing-payment-request', loadComponent: () => import('./pages/outgoing-payment/outgoing-payment-request/outgoing-payment-request.component').then((m) => m.OutgoingPaymentRequestComponent )},
                  { path: 'approval-workflow-engine', data: { animation: 'ApprovalWorkflowEnginePage' }, loadComponent: () => import('./pages/expense-approval-workflow-engine/expense-approval-workflow-engine.component').then((m) => m.ExpenseApprovalWorkflowEngineComponent )},
                  { path: 'approval-workflow-engine/approval-workflow-engine-request', loadComponent: () => import('./pages/expense-approval-workflow-engine/expense-approval-workflow-engine-request/expense-approval-workflow-engine-request.component').then((m) => m.ExpenseApprovalWorkflowEngineRequest )},
                  { path: 'payment-request', loadComponent: () => import('./pages/payment-requests/payment-request.component').then((m) => m.ExpensePaymentRequestComponent )},
                  { path: 'payment-request/expense-payment', loadComponent: () => import('./pages/payment-requests/expense-payments/expense-payment.component').then((m) => m.ExpensePaymentComponent )},
            ]
      }
]