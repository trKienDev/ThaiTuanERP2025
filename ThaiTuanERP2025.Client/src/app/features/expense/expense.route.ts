import { Routes } from "@angular/router";
import { ExpenseComponent } from "./expense.component";
import { FollowingExpensePaymentsPanelComponent } from "./pages/expense-payment-shell-page/following-expense-payments/following-expense-payments.component";
import { ExpensePaymentRequestPanelComponent } from "./pages/expense-payment-shell-page/expense-payment-request/expense-payment-request.component";
import { FollowingOutgoingPaymentComponent } from "./pages/outgoing-payment-shell-page/following-outgoing-payment/following-outgoing-payment.component";
import { OutgoingPaymentRequestComponent } from "./pages/outgoing-payment-shell-page/outgoing-payment-request/outgoing-payment-request.component";
import { OutgoingBankAccountComponent } from "./pages/bank-account-shell-page/outgoing-bank-account/outgoing-bank-account.component";

export const expenseRoutes: Routes = [
      {
            path: '',
            component: ExpenseComponent,
            children: [
                  { path: '', redirectTo: 'expense-payment-shell', pathMatch: 'full'},
                  { path: 'supplier', data: { animation: 'SupplierPage' }, loadComponent: () => import('./pages/suppliers/supplier.component').then((m) => m.ExpenseSupplierComponent )},
                  { path: 'invoice', data: { animation: 'InvoicePage' }, loadComponent: () => import('./pages/invoices/invoice.component').then((m) => m.ExpenseInvoiceComponent )},
                  { path: 'invoice/request', data: { animation: 'InvoiceRequestPage' }, loadComponent: () => import('./pages/invoices/invoice-request/invoice-request-page.component').then((m) => m.InvoiceRequestPageComponent )},
                  { 
                        path: 'expense-payment-shell', 
                        loadComponent: () => import('./pages/expense-payment-shell-page/expense-payment-shell-page.component').then((m) => m.ExpensePaymentShellPageComponent), 
                        children: [
                              { path: '', redirectTo: 'following-payments', pathMatch: 'full' },
                              { path: 'following-payments', component: FollowingExpensePaymentsPanelComponent },
                              { path: 'payment-request', component: ExpensePaymentRequestPanelComponent },
                        ]
                  },
                  { 
                        path: 'outgoing-payment-shell', 
                        loadComponent: () => import('./pages/outgoing-payment-shell-page/outgoing-payment-shell-page.component').then((m) => m.OutgoingPaymentShellPageComponent), 
                        children: [
                              { path: '', redirectTo: 'following-outgoing-payments', pathMatch: 'full' },
                              { path: 'following-outgoing-payments', component: FollowingOutgoingPaymentComponent },
                              { path: 'outgoing-payment-request/:id', component: OutgoingPaymentRequestComponent },
                        ]
                  },
                  { 
                        path: 'bank-account-shell-page',
                        loadComponent: () => import('./pages/bank-account-shell-page/bank-account-shell-page.component').then((m) => m.BankAccountShellPageComponent),
                        children: [
                              { path: '', redirectTo: 'outgoing-bank-account', pathMatch: 'full' },
                              { path: 'outgoing-bank-account', component: OutgoingBankAccountComponent },
                              { path: 'supplier-bank-account', loadComponent: () => import('./pages/bank-account-shell-page/supplier-bank-account/supplier-bank-account.component').then((m) => m.SupplierBankAccountComponent) },
                        ]
                  },
                  { path: 'approval-workflow-engine', data: { animation: 'ApprovalWorkflowEnginePage' }, loadComponent: () => import('./pages/expense-approval-workflow-engine/expense-approval-workflow-engine.component').then((m) => m.ExpenseApprovalWorkflowEngineComponent )},
                  { path: 'approval-workflow-engine/approval-workflow-engine-request', loadComponent: () => import('./pages/expense-approval-workflow-engine/expense-approval-workflow-engine-request/expense-approval-workflow-engine-request.component').then((m) => m.ExpenseApprovalWorkflowEngineRequest )},
            ]
      }
]