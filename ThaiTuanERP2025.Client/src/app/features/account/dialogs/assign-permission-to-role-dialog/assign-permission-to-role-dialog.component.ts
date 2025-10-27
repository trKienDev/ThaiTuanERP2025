import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder } from "@angular/forms";
import { RoleDto } from "../../models/role.model";
import { PermissionFacade } from "../../facades/permission.facade";
import { PermissionService } from "../../services/permission.service";
import { PermissionDto } from "../../models/permission.model";

@Component({
      selector: 'assign-permission-to-role-dialog',
      templateUrl: './assign-permission-to-role-dialog.component.html',
      styleUrls: ['./assign-permission-to-role-dialog.component.scss'],
      standalone: true,
      imports: [ CommonModule ]
})
export class AssignPermissionToRoleDialogComponent implements OnInit {
      private toastService = inject(ToastService);    
      private matDialogRef = inject(MatDialogRef<AssignPermissionToRoleDialogComponent>);
      private formBuilder = inject(FormBuilder);
      public role!: RoleDto;
      public submitting = false;
      private readonly permissionFacade = inject(PermissionFacade);
      public permissions$ = this.permissionFacade.permissions$;
      private readonly permissionService = inject(PermissionService);
      public permissionsByRole: PermissionDto[] = [];

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: RoleDto
      ) {}

      ngOnInit(): void {
            this.role = this.data;
            console.log('Role in dialog:', this.role);
            this.loadPermissionsByRole();
      }

      loadPermissionsByRole(): void {
            this.permissionService.getByRoleId(this.role.id).subscribe({
                  next: (permissions) => {
                        this.permissionsByRole = permissions;
                  },
                  error: (error) => {
                        this.toastService.errorRich("Không thể lấy danh sách permission của role này");
                  }
            });
      }

      closeDialog(): void {
            this.matDialogRef.close();
      }
}