import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { SupplierService } from "../../../services/supplier.service";
import { CreateSupplierRequest } from "../../../models/supplier.model";
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { MatAutocompleteModule } from "@angular/material/autocomplete";

@Component({
      selector: 'supplier-request-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
            MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule, MatAutocompleteModule,
            FormsModule, MatInputModule
      ],
      templateUrl: './supplier-request-dialog.component.html',
})
export class SupplierRequestDialogComponent {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<SupplierRequestDialogComponent>);
      private supplierService = inject(SupplierService);

      saving = false;
      errorMessages: string[] = [];


      supplierForm = this.formBuilder.group({
            name: ['', {
                  validators: [ Validators.required, Validators.maxLength(256)],
            }],
            taxCode: [''],
            isActive: [true],
      });
      get supplierRequestForm() { return this.supplierForm.controls; }

      submit(): void {
            this.errorMessages = [];
            if(this.supplierForm.invalid) {
                  this.supplierForm.markAllAsTouched();
                  return;
            }

            const payload = this.supplierForm.getRawValue() as CreateSupplierRequest;

            this.saving = true;

            this.supplierService.create(payload).pipe(
                  catchError(err => {
                        this.errorMessages = handleHttpError(err);
                        this.saving = false;
                        return of(null);
                  })
            ).subscribe((created) => {
                  if(!created) return;
                  this.dialogRef.close(true);
            })
      }

      cancel(): void {
            this.dialogRef.close();
      }
}