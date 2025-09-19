import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { UserFacade } from "../../../facades/user.facade";
import { UserOptionStore } from "../../../options/user-dropdown-options.store";
import { UserDto } from "../../../models/user.model";

@Component({
      selector: 'member-manager-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule ,KitDropdownComponent],
      templateUrl: './member-manager-dialog.component.html'
})
export class MemberManagerDialog implements OnInit {
      private dialogRef = inject(MatDialogRef<MemberManagerDialog>);
      private formBuilder = inject(FormBuilder);
      user!: UserDto;
      
      private userFacade = inject(UserFacade);
      private userOptionsStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionsStore.option$;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: UserDto 
      ) {}

      submitting = false;

      form = this.formBuilder.group({
            managerIds: this.formBuilder.control<string[]>([], { nonNullable: true })
      })

      ngOnInit(): void {
            if(this.data) {
                  this.user = this.data;
                  this.form.patchValue({
                        managerIds: this.data.managerId
                  })
            }
      }

      onManagerSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt.id;
            const ctrl = this.form.controls.managerIds;
            const current = ctrl.getRawValue() ?? [];
            if (!current.includes(id)) ctrl.setValue([...current, id]);
            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }

      close(): void {
            this.dialogRef.close();
      }
}