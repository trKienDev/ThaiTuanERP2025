import { CommonModule } from "@angular/common";
import { Component, OnInit, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ExpenseApprovalWorkflowService } from "../../services/expense-approval-workflow.service";
import { CreateExpenseApprovalWorkflowRequest, ExpenseApprovalWorkflowDto, WorkflowStepRequest } from "../../models/expense-approval-workflow.model";
import { NamingExpenseApprovalWorkflowDialogComponent } from "./naming-approval-workflow-dialog/naming-approval-workflow-dialog.component";
import { EAFStepEditorDialogComponent, StepEditorResult } from "./step-editor-dialog/step-editor-dialog.component";
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../core/utils/handle-http-errors.util";

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
      imports: [CommonModule, MatDialogModule, MatMenuModule, MatIconModule, MatButtonModule ],
      templateUrl: './expense-approval-workflow-engine.component.html',
      styleUrl: './expense-approval-workflow-engine.component.scss',
})
export class ExpenseApprovalWorkflowEngineComponent implements OnInit {
      errorMessages:string[] = [];

      // ====== state cho danh sách ======
      loading = signal(false);
      workflows = signal<ExpenseApprovalWorkflowDto[]>([]);

      // canvas state
      isOpen = signal(false);
      steps = signal<Step[]>([]);

      // lưu tên workflow (đã có dialog đặt tên)
      workflowName = '';
      
      constructor(
            private expenseApprovalWorkflowService: ExpenseApprovalWorkflowService,
            private dialog: MatDialog
      ) {}

      // ====== load danh sách khi vào trang ======
      ngOnInit(): void {
            this.loadExpenseApprovalWorkflows();
      }

      loadExpenseApprovalWorkflows() {
            this.loading.set(true);
            this.expenseApprovalWorkflowService.getAll().subscribe({
                  next: list => {
                        this.workflows.set(list ?? []);
                        this.loading.set(false);
                  },
                  error: _ => this.loading.set(false)
            })
      }

      // mở/đóng canvas
      open() { this.isOpen.set(true); }
      close() { this.isOpen.set(false); }

      // ====== click “Sửa” một workflow từ danh sách ======
      editWorkflow(workflow: ExpenseApprovalWorkflowDto) {
            // mở canvas và (tuỳ bạn) load steps vào canvas để chỉnh
            this.workflowName = workflow.name;
            this.steps.set(
                  (workflow.steps ?? []).sort((a, b) => a.order - b.order).map(s => ({
                        id: crypto.randomUUID(),
                        title: s.title,
                        approverIds: s.candidateUserIds, // tùy chỉnh
                        flowType: s.flowType as StepFlowType,
                        slaHours: s.slaHours ?? 8
                  }))
            );
            this.open();
      }
      removeStep(index: number) {
            this.steps.update(list => list.filter((_, i) => i !== index));
      }

      // dialog đặt tên (giữ nguyên)
      openNameDialog() {
            const ref = this.dialog.open(NamingExpenseApprovalWorkflowDialogComponent, {
                  data: { defaultName: this.workflowName || '' }, // sửa 'defaulName' -> 'defaultName' nếu dialog nhận prop này
                  width: '480px',
                  disableClose: true,
            });
            ref.afterClosed().subscribe(name => {
                  if (!name) return;
                  this.workflowName = name;
                  this.saveWorkflow();
            });
      }

      // ==== Tạo step mới ===
      openStepDialog() {
            this.dialog.open(EAFStepEditorDialogComponent, {
                  width: '640px',
                  panelClass: 'custom-dialog-modal',
                  disableClose: true,
                  data: {
                        initial: {
                              title: '',
                              approverIds: [],
                              flowType: 'sequential',
                              slaHours: 8,
                        }
                  }
            }).afterClosed().subscribe((res?: StepEditorResult) => {
                  if (!res) return;
                  const step: Step = {
                        id: crypto.randomUUID(),
                        title: res.title,
                        approverIds: res.approverIds,
                        flowType: res.flowType,
                        slaHours: res.slaHours ?? 8
                  };
                  this.steps.update(list => [...list, step]);
            });
      }
      // ==== Edit step ====
      editStep(index: number) {
            const current = this.steps()[index];
            if(!current) return;
            
            this.dialog.open(EAFStepEditorDialogComponent, {
                  width: '640px',
                  disableClose: true,
                  data: {
                        initial: {
                              title: current.title,
                              approverIds: current.approverIds,
                              flowType: current.flowType,
                              slaHours: current.slaHours ?? 8
                        }
                  }
            }).afterClosed().subscribe((res?: StepEditorResult) => {
                  if(!res) return;
                  this.steps.update(list => {
                        const copy = [...list];
                        copy[index] = {
                              ...copy[index],
                              title: res.title,
                              approverIds: res.approverIds,
                              flowType: res.flowType,
                              slaHours: res.slaHours ?? 8
                        };
                        return copy;
                  })
            })
      }

      // gửi server
      saveWorkflow() {
            const stepRequests: WorkflowStepRequest[] = this.steps().map((s, idx) => ({
                  title: s.title,
                  order: idx + 1,
                  candidateUserIds: s.approverIds,
                  flowType: s.flowType,
                  slaHours: s.slaHours ?? 8
            }));

            const payload: CreateExpenseApprovalWorkflowRequest = {
                  name: this.workflowName,
                  isActive: true,
                  steps: stepRequests
            };

            this.expenseApprovalWorkflowService.create(payload).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        return of(null);
                  })
            ).subscribe((created) => {
                  if(!created) return;
                  alert('Tạo luồng duyệt thành công');
                  this.close();
                  this.loadExpenseApprovalWorkflows();
            });
      }
}
