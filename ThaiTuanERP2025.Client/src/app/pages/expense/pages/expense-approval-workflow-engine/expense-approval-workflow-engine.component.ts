import { CommonModule } from "@angular/common";
import { Component, signal } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators, ValidatorFn, FormControl } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatChipsModule }      from '@angular/material/chips';
import { MatOptionModule }     from '@angular/material/core';
import { UserDto } from "../../../account/models/user.model";
import { UserService } from "../../../account/services/user.service";
import { startWith } from "rxjs";
import { ExpenseApprovalWorkflowService } from "../../services/expense-approval-workflow.service";
import { CreateExpenseApprovalWorkflowRequest, WorkflowStepRequest } from "../../models/expense-approval-workflow.model";
import { NamingExpenseApprovalWorkflowDialogComponent } from "./naming-approval-workflow-dialog/naming-approval-workflow-dialog.component";
import { MatDialog, MatDialogModule } from "@angular/material/dialog";

type StepFlowType = 'any' | 'sequential' | 'all';
interface Step {
      id: string; 
      title: string;
      approverIds: string[];
      flowType: StepFlowType;
      slaHours: number | null;
}

/** Validator: mảng phải có ít nhất 1 phần tử */
const arrayRequired = (): ValidatorFn => (control: AbstractControl) => {
      const value = control.value as unknown[];
      return value && Array.isArray(value) && value.length > 0 ? null : { required: true };
}

@Component({
      selector: 'expense-approval-workflow-engine',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule,
            MatChipsModule, MatIconModule, MatOptionModule, MatDialogModule
      ],
      templateUrl: './expense-approval-workflow-engine.component.html',
      styleUrl: './expense-approval-workflow-engine.component.scss',
})
export class ExpenseApprovalWorkflowEngineComponent {
      isOpen = signal(false); // trạng thái đóng/mở canvas

      isStepModalOpen = signal(false); // popup add step modal
      steps = signal<Step[]>([]); // nocde 'Gửi xử lý' luôn có sẵn; các step sau sẽ được hiển thị khi bấm "+"

      // --- Users & autocomplete ---
      allUsers: UserDto[] = [];
      filteredUsers = signal<UserDto[]>([]);
      selectedApprovers = signal<UserDto[]>([]);
      approverSearchCtrl = new FormControl<string>('', { nonNullable: true });

      stepForm: FormGroup; // form modal

      constructor(
            private formBuilder: FormBuilder, 
            private userService: UserService, 
            private expenseApprovalWorkflowService: ExpenseApprovalWorkflowService,
            private dialog: MatDialog
      ) {
            this.stepForm = this.formBuilder.group({
                  title: ['', Validators.required],
                  approverIds: this.formBuilder.control<string[]>([], arrayRequired()), 
                  flowType: ['sequential' as StepFlowType, Validators.required],
                  slaHours: [8],
            });

            // Tải users 1 lần rồi filter ở client
            this.userService.getAllUsers().subscribe(list => {
                  this.allUsers = list ?? [];
                  this.filteredUsers.set(this.allUsers);
            });

            // filter theo query (tên / username / position)
            this.approverSearchCtrl.valueChanges.pipe(startWith('')).subscribe(q => {
                  const query = this.normalize(q || '');
                  const res = !query
                        ? this.allUsers
                        : this.allUsers.filter(u =>
                        [u.fullName, u.username, u.position]
                              .filter(Boolean)
                              .some(f => this.normalize(String(f)).includes(query))
                        );
                  // loại trừ đã chọn
                  const chosenIds = new Set(this.selectedApprovers().map(x => x.id!));
                  this.filteredUsers.set(res.filter(u => !chosenIds.has(u.id!)));
            });
      }

      // MODAL: NAMING APPROVAL WORKFLOW DIALOG
      workflowName = '';
      openNameDialog() {
            const ref = this.dialog.open(NamingExpenseApprovalWorkflowDialogComponent, {
                  data: { defaulName: this.workflowName || '' },
                  width: '480px',
                  disableClose: true,
            });
            ref.afterClosed().subscribe(name => {
                  if(!name) return;
                  this.workflowName = name;
                  this.saveWorkflow();
            });
      }


      // mở/đóng canvas
      open() { this.isOpen.set(true); }
      close() { this.isOpen.set(false); }

      // mở modal khi bấm "+"
      openStepModal() {
            this.isStepModalOpen.set(true);
            // reset về mặc định
            this.resetStepForm();
      }
      // đóng modal + xóa dữ liệu đang nhập
      closeStepModal() {
            this.isStepModalOpen.set(false);
            this.resetStepForm();
      }
      private resetStepForm() {
            this.stepForm.reset({
                  title: '',
                  approverIds: [],
                  flowType: 'sequential',
                  slaHours: 8,
            });
            this.selectedApprovers.set([]),
            this.approverSearchCtrl.setValue('');
            this.filteredUsers.set(this.allUsers);
      }
 
      // helper
      private normalize(s: string) {
            return (s ?? '').toString().toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');
      }
      allowOnlyNumberKeys(evt: KeyboardEvent) {
            const allow = ['Backspace','Delete','ArrowLeft','ArrowRight','Tab','Home','End'];
            if (allow.includes(evt.key)) return;
            if (!/^\d$/.test(evt.key)) evt.preventDefault();
      }

      // chọn user từ autocomplete
      selectApprover(u: UserDto) {
            if (!u?.id) return;
            const chosen = this.selectedApprovers();
            if (!chosen.some(x => x.id === u.id)) {
                  const next = [...chosen, u];
                  this.selectedApprovers.set(next);
                  this.stepForm.get('approverIds')!.setValue(next.map(x => x.id!));
            }
            // clear ô tìm kiếm để chọn tiếp
            this.approverSearchCtrl.setValue('');
      }
      // xóa 1 approver (chip)
      removeApprover(userId: string | undefined) {
            if (!userId) return;
            const next = this.selectedApprovers().filter(x => x.id !== userId);
            this.selectedApprovers.set(next);
            this.stepForm.get('approverIds')!.setValue(next.map(x => x.id!));
            // refresh danh sách gợi ý
            this.approverSearchCtrl.updateValueAndValidity({ emitEvent: true });
      }

      // Lưu bước mới
      saveStep() {
            if(this.stepForm.invalid) {
                  this.stepForm.markAllAsTouched();
                  return;
            }

            const raw = this.stepForm.value;
            let sla = Number(raw.slaHours);
            if (!Number.isFinite(sla) || sla < 0) sla = 8;

           const step: Step = {
                  id: crypto.randomUUID(),
                  title: (raw.title as string).trim(),
                  approverIds: (raw.approverIds as string[]) ?? [],
                  flowType: raw.flowType as StepFlowType,
                  slaHours: sla,
            };
            this.steps.update(list => [...list, step]);
            this.closeStepModal();
      }

      saveWorkflow() {
            // map từ state 'steps()' (id, title, approverIds, flowType, slaHours) -> request gửi server
            const stepRequests: WorkflowStepRequest[] = this.steps().map((s, idx) => ({
                  title: s.title,
                  order: idx + 1,
                  candidateUserIds: s.approverIds,
                  flowType: s.flowType,                // 'any' | 'sequential' | 'all'
                  slaHours: s.slaHours ?? 8
            }));

            const payload: CreateExpenseApprovalWorkflowRequest = {
                  name: this.workflowName,
                  isActive: true,
                  steps: stepRequests
            };

            this.expenseApprovalWorkflowService.create(payload).subscribe({
                  next: (res) => {
                        if(res?.isSuccess) {
                              alert('Tạo luồng duyệt thành công');
                              this.close();
                        } else {
                              alert('Tạo luồng duyệt thất bại');
                              console.error('lỗi: ', res.message);
                        }
                  },
                  error: (err) => {
                        console.error('Tạo luồng duyệt thất bại', err)
                  }
            })
      }
}