import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, FormsModule } from "@angular/forms";
import { RoleDto } from "../../models/role.model";
import { PermissionDto } from "../../models/permission.model";
import { firstValueFrom } from "rxjs";
import { trigger, transition, style, animate } from "@angular/animations";
import { PermissionApiService } from "../../services/api/permission-api.service";
import { RoleApiService } from "../../services/api/role-api.service";

@Component({
      selector: 'assign-permission-to-role-dialog',
      templateUrl: './assign-permission-to-role-dialog.component.html',
      styleUrls: ['./assign-permission-to-role-dialog.component.scss'],
      standalone: true,
      imports: [ CommonModule, FormsModule ],
      animations: [
            trigger('slideList', [
                  transition(':enter', [
                        style({ opacity: 0, transform: 'translateX(15px)' }),
                        animate('250ms ease-out', style({ opacity: 1, transform: 'translateX(0)' }))
                  ]),
                  transition(':leave', [
                        animate('250ms ease-in', style({ opacity: 0, transform: 'translateX(-15px)' }))
                  ])
            ]),
      ]
})
export class AssignPermissionToRoleDialogComponent implements OnInit {
      private readonly toastService = inject(ToastService);    
      private readonly matDialogRef = inject(MatDialogRef<AssignPermissionToRoleDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      public role!: RoleDto;
      public submitting = false;
      private readonly permissionApi = inject(PermissionApiService);
      private readonly roleApi = inject(RoleApiService);
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
                  const all = await firstValueFrom(this.permissionApi.getAll());
                  const byRole = await firstValueFrom(this.permissionApi.getByRoleId(this.role.id));

                  this.selectedPermissions = byRole;
                  this.availablePermissions = all.filter(p => !byRole.some(br => br.id === p.id));
            } catch (error) {
                  this.toastService.errorRich("Không thể lấy danh sách permission");
                  return
            }
      }

      toggleAvailable(permission: PermissionDto) {
            console.log('toggleAvailable');
            this.toggleSelect(this.selectedAvailable, permission);
      }

      toggleAssigned(permission: PermissionDto) {
            console.log('toggleAssigned')
            this.toggleSelect(this.selectedAssigned, permission);
      }

      private toggleSelect(collection: PermissionDto[], permission: PermissionDto) {
            const idx = collection.findIndex(p => p.id === permission.id);
            if (idx >= 0) collection.splice(idx, 1);
            else collection.push(permission);
      }

      moveToAssigned() {
            this.selectedPermissions.push(...this.selectedAvailable);
            this.availablePermissions = this.availablePermissions.filter(
                  p => !this.selectedAvailable.includes(p)
            );
            this.selectedAvailable = [];
            this.selectedAssigned = []; 
      }

      moveToAvailable() {
            this.availablePermissions.push(...this.selectedAssigned);
            this.selectedPermissions = this.selectedPermissions.filter(
                  p => !this.selectedAssigned.includes(p)
            );
            this.selectedAssigned = [];
            this.selectedAvailable = [];
      }

      async submit(): Promise<void> {
            this.submitting = true;
            try {
                  const ids = this.selectedPermissions.map(p => p.id);
                  await firstValueFrom(this.roleApi.assignPermissions(this.role.id, ids));
                  this.toastService.successRich('Phân quyền thành công');
                  this.matDialogRef.close(true);
                  return;
            } catch(error) {
                  this.toastService.errorRich('Lỗi khi phân quyền');
            } finally {
                  this.submitting = false;
            }
      }

      closeDialog(): void {
            this.matDialogRef.close();
      }
}