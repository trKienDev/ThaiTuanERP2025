import { CommonModule } from "@angular/common";
import { Component, inject, OnInit, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { RouterLink } from "@angular/router";

type StepFlowType = 'any' | 'sequential' | 'all';
interface Step {
      id: string;
      title: string;
      approverIds: string[];
      flowType: StepFlowType;
      slaHours: number | null;
}

@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule, RouterLink],
      templateUrl: './expense-approval-workflow-engine.component.html',
})
export class ExpenseApprovalWorkflowEngineComponent {
      private readonly toastService = inject(ToastService);

      errorMessages:string[] = [];
      
      loading = signal(false);




}
