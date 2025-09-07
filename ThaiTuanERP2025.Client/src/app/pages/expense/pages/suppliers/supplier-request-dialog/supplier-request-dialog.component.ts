import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { SupplierService } from "../../../services/supplier.service";
import { CreateSupplierRequest } from "../../../models/supplier.model";
import { catchError, debounceTime, distinctUntilChanged, first, map, Observable, of, switchMap } from "rxjs";
import { handleHttpError } from "../../../../../core/utils/handle-http-errors.util";
import { MatAutocompleteModule } from "@angular/material/autocomplete";

@Component({
      selector: 'supplier-request-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
            MatInputModule, MatCheckboxModule, MatButtonModule, MatSelectModule, MatAutocompleteModule,
            FormsModule, MatInputModule
      ],
      templateUrl: './supplier-request-dialog.component.html',
      styleUrl: './supplier-request-dialog.component.scss'
})
export class SupplierRequestDialogComponent {
      private formBuilder = inject(FormBuilder);
      private dialogRef = inject(MatDialogRef<SupplierRequestDialogComponent>);
      private supplierService = inject(SupplierService);

      saving = false;
      errorMessages: string[] = [];

      private supplierNameAvailableValidator: AsyncValidatorFn = (control: AbstractControl): Observable<any> => {
            const value = (control.value ?? '').trim();
            if(!value) return of(null);
            return of(value).pipe(
                  debounceTime(300),
                  distinctUntilChanged(),
                  switchMap(name => this.supplierService.checkNameAvailable(name).pipe(
                        map(isAvailable => (isAvailable ? null : {nameTaken: true})),
                        catchError(() => of(null)) 
                  )),
                  first()
            );
      };

      supplierForm = this.formBuilder.group({
            name: ['', {
                  validators: [ Validators.required, Validators.maxLength(256)],
                  asyncValidators: [ this.supplierNameAvailableValidator ],
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
                  this.dialogRef.close(created);
            })
      }

      cancel(): void {
            this.dialogRef.close();
      }

}