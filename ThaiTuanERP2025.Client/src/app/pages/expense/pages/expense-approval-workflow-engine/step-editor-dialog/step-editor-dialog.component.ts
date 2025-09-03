import { CommonModule } from "@angular/common";
import { Component, Inject } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatChipsModule } from "@angular/material/chips";
import { MatOptionModule } from "@angular/material/core";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { UserDto } from "../../../../account/models/user.model";
import { UserService } from "../../../../account/services/user.service";
import { startWith } from "rxjs";
import { MatIconModule } from "@angular/material/icon";

type StepFlowType = 'any' | 'sequential' | 'all';
export interface StepEditorData {
      initial?: {
            title: string;
            approverIds: string[];
            flowType: StepFlowType;
            slaHours: number | null;
      };
}
export interface StepEditorResult {
      title: string;
      approverIds: string[];
      flowType: StepFlowType;
      slaHours: number;
}

const arrayRequired = (): ValidatorFn => (c: AbstractControl) => Array.isArray(c.value) && c.value.length > 0 ? null : { required: true };

@Component({
      selector: 'approval-workflow-step-editor-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule,
    MatAutocompleteModule, MatChipsModule, MatOptionModule, MatIconModule],
      templateUrl: './step-editor-dialog.component.html',
      styleUrl: './step-editor-dialog.component.scss'
})
export class EAFStepEditorDialogComponent {
      allUsers: UserDto[] = [];
      filteredUsers: UserDto[] = [];
      selectedApprovers: UserDto[] = [];
      approverSearchCtrl = new FormControl<string>('', { nonNullable: true });

      form!: FormGroup; 

      constructor(
            private formBuiler: FormBuilder,
            private userService: UserService,
            private dialogRef: MatDialogRef<EAFStepEditorDialogComponent, StepEditorResult>,
            @Inject(MAT_DIALOG_DATA) public data: StepEditorData,
      ) {
            
            this.form = this.formBuiler.group({
                  title: ['', Validators.required],
                  approverIds: this.formBuiler.control<string[]>([], arrayRequired()),
                  flowType: this.formBuiler.control<StepFlowType>('sequential', { validators: Validators.required }),
                  slaHours: this.formBuiler.control<number | null>(8),
            })
            // load users
            this.userService.getAllUsers().subscribe(list => {
                  this.allUsers = list ?? [];
                  this.filteredUsers = [...this.allUsers];
                  // hydrate initial approvers if editing
                  if(data?.initial?.approverIds?.length) {
                        this.selectedApprovers = this.allUsers.filter(u => data.initial?.approverIds.includes(u.id!));
                        this.form.get('approverIds')!.setValue(this.selectedApprovers.map(u => u.id!));
                  }
            });

            // init values if editing
            if(data?.initial) {
                  this.form.patchValue({
                        title: data.initial.title ?? '',
                        flowType: data.initial.flowType ?? 'sequential',
                        slaHours: data.initial.slaHours ?? 8,
                  }, { emitEvent: false })
            }

            // filter users
            this.approverSearchCtrl.valueChanges.pipe(startWith('')).subscribe(q => {
                  const query = this.normalize(q || '');
                  const base = query ? this.allUsers.filter(u => [u.fullName, u.username, u.position].filter(Boolean).some(f => this.normalize(String(f)).includes(query))) : this.allUsers;
                  const chosen = new Set(this.selectedApprovers.map(x => x.id));
                  this.filteredUsers = base.filter(u => !chosen.has(u.id));
            });
      }

      selectApprover(u: UserDto) {
            if(!u?.id) return;
            if(!this.selectedApprovers.some(x => x.id === u.id)) {
                  this.selectedApprovers = [...this.selectedApprovers, u];
                  this.form.get('approverIds')!.setValue(this.selectedApprovers.map(x => x.id!));
            }
            this.approverSearchCtrl.setValue('');
      }

      removeApprover(id?: string) {
            if(!id) return;
            this.selectedApprovers = this.selectedApprovers.filter(x => x.id !== id);
            this.form.get('approverIds')!.setValue(this.selectedApprovers.map(x => x.id!));
            this.approverSearchCtrl.updateValueAndValidity({ emitEvent: true })
      }

      allowOnlyNumberKeys(evt: KeyboardEvent) {
            const allow = ['Backspace','Delete','ArrowLeft','ArrowRight','Tab','Home','End'];
            if (allow.includes(evt.key)) return;
            if (!/^\d$/.test(evt.key)) evt.preventDefault();
      }

      cancel() {
            this.dialogRef.close();
      }

      save() {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const v = this.form.value;
            this.dialogRef.close({
                  title: String(v.title).trim(),
                  approverIds: v.approverIds || [],
                  flowType: v.flowType as StepFlowType,
                  slaHours: Number.isFinite(v.slaHours) && (v.slaHours as number) >= 0 ? (v.slaHours as number) : 8
            });
      }



      private normalize(s: string) {
            return (s ?? '').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');
      }
}