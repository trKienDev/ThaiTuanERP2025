import { CommonModule } from "@angular/common";
import { Component, inject  } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "../../components/department-request-dialog/department-request-dialog.component";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto } from "../../models/department.model";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { SetParentDepartmentDialogComponent } from "../../components/set-parent-department-dialog/set-parent-department-dialog.component";
import { DepartmentManagerDialogComponent } from "../../components/department-manager-dialog/department-manager-dialog.component";
import { HasPermissionDirective } from "../../../../core/auth/auth.directive";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent, HasPermissionDirective],
      templateUrl: './account-department.component.html',
})
export class AccountDepartmentComponent {      
      private readonly dialog = inject(MatDialog);
      private readonly departmentFacade = inject(DepartmentFacade);

      departments$ = this.departmentFacade.departments$;
      
      trackById(index: number, item: DepartmentDto) { return item.id; }

      openDeparmentRequestDialog(): void {
            const dialog = this.dialog.open(DepartmentRequestDialog);
            dialog.afterClosed().subscribe();
      }

      addDepartmentManager(dept: DepartmentDto): void {
            this.dialog.open(DepartmentManagerDialogComponent, {
                  data: dept
            })
      }

      editDepartment(department: DepartmentDto): void {
            this.dialog.open(DepartmentRequestDialog, {
                  data: department
            });            
      }

      setParentDepartment(department: DepartmentDto): void {
            this.dialog.open(SetParentDepartmentDialogComponent, {
                  data: department.id
            });
      }

      buildDepartmentActions(dept: DepartmentDto): ActionMenuOption[] {
            return [
                  { label: '👨🏻‍💼 Chỉnh sửa quản lý', action: () => this.addDepartmentManager(dept) },
                  { label: '👥 Thiết lập phòng ban cha', action: () => this.setParentDepartment(dept) },
                  { label: '⚙️ Sửa' },
                  { label: '⛔ Xóa', color: 'red' },
            ]
      }
}