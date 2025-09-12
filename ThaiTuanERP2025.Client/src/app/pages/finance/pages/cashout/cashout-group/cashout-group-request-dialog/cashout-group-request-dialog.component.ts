import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { CashoutGroupDto, CreateCashoutGroupRequest } from "../../../../models/cashout-group.model";
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { MatSelectModule } from "@angular/material/select";
import { CashoutGroupService } from "../../../../services/cashout-group.service";

@Component({
      selector: 'cashout-group-request-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
            MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule 
      ],
      templateUrl: './cashout-group-request-dialog.component.html',
      styleUrl: './cashout-group-request-dialog.component.scss'
})
export class CashoutGroupRequestDialogComponent implements OnInit {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<CashoutGroupRequestDialogComponent>);
      private cashoutGroupService = inject(CashoutGroupService);

      saving = false;
      errorMessages: string[] = [];

      parentOptions: CashoutGroupDto[] = [];

      ngOnInit(): void {
            this.loadCashoutGroups();
      }


      form = this.formBuilder.group({
            name: ['', { validators: [Validators.required, Validators.maxLength(200)], updateOn: 'blur' }],
            description: [''],
            isActive: [true],
            parentId: [null],
      });

      get cashoutGroupRequestForm() {
            return this.form.controls;
      }

      loadCashoutGroups(): void {
            this.cashoutGroupService.getAll().subscribe({
                  next: (parents) => {
                        this.parentOptions = parents;
                  }, error: err => alert(handleHttpError(err).join('\n'))
            })
      }

      submit(): void {
            this.errorMessages = [];
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            const payload = this.form.getRawValue() as CreateCashoutGroupRequest;

            this.saving = true;
            this.cashoutGroupService.create(payload).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        this.saving = false;
                        return of(null);
                  })
            ).subscribe((created) => {
                  if(!created) return;
                  this.dialogRef.close('created');
            })
      }

      cancel(): void {
            this.dialogRef.close();
      }
}