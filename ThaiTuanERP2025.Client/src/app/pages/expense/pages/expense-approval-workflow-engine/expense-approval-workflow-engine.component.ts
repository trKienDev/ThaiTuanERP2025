import { CommonModule } from "@angular/common";
import { Component, signal } from "@angular/core";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";
import { ExpenseApprovalWorkflowService } from "../../services/expense-approval-workflow.service";
import { CreateExpenseApprovalWorkflowRequest, WorkflowStepRequest } from "../../models/expense-approval-workflow.model";
import { NamingExpenseApprovalWorkflowDialogComponent } from "./naming-approval-workflow-dialog/naming-approval-workflow-dialog.component";
import { EAFStepEditorDialogComponent, StepEditorResult } from "./step-editor-dialog/step-editor-dialog.component";

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
      imports: [CommonModule, MatDialogModule],
      templateUrl: './expense-approval-workflow-engine.component.html',
      styleUrl: './expense-approval-workflow-engine.component.scss',
})
export class ExpenseApprovalWorkflowEngineComponent {
      // canvas state
      isOpen = signal(false);
      steps = signal<Step[]>([]);

      // lưu tên workflow (đã có dialog đặt tên)
      workflowName = '';

      constructor(
            private expenseApprovalWorkflowService: ExpenseApprovalWorkflowService,
            private dialog: MatDialog
      ) {}

      // mở/đóng canvas
      open() { this.isOpen.set(true); }
      close() { this.isOpen.set(false); }

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

      // mở dialog thêm step
      openStepDialog() {
            this.dialog.open(EAFStepEditorDialogComponent, {
                  width: '640px',
                  disableClose: true,
                  data: {} // hoặc { initial: step } cho chế độ Edit
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

            this.expenseApprovalWorkflowService.create(payload).subscribe({
                  next: (res) => {
                        if (res?.isSuccess) {
                        alert('Tạo luồng duyệt thành công');
                        this.close();
                        } else {
                        alert('Tạo luồng duyệt thất bại');
                        console.error('lỗi: ', res?.message);
                        }
                  },
                        error: (err) => {
                        console.error('Tạo luồng duyệt thất bại', err);
                  }
            });
      }
}
