import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder } from "@angular/forms";
import { RoleDto } from "../../models/role.model";
import { PermissionService } from "../../services/permission.service";
import { PermissionDto } from "../../models/permission.model";
import { firstValueFrom } from "rxjs";

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
      private readonly permissionService = inject(PermissionService);
      public permissionsByRole: PermissionDto[] = [];

      availablePermissions: PermissionDto[] = [];
      selectedPermissions: PermissionDto[] = [];

      selectedAvailable: PermissionDto[] = [];
      selectedAssigned: PermissionDto[] = [];

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: RoleDto
      ) {}

      async ngOnInit(): Promise<void> {
            this.role = this.data;
            await this.loadPermissions();
      }

      private async loadPermissions() {
            try {
                  const all = await firstValueFrom(this.permissionService.getAll());
                  const byRole = await firstValueFrom(this.permissionService.getByRoleId(this.role.id));

                  this.selectedPermissions = byRole;
                  this.availablePermissions = all.filter(p => !byRole.some(br => br.id === p.id));
            } catch (error) {
                  this.toastService.errorRich("Không thể lấy danh sách permission");
                  return
            }
      }

      closeDialog(): void {
            this.matDialogRef.close();
      }
}