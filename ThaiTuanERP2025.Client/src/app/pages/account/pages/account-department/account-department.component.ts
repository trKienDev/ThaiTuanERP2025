import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "./department-request/department-request.component";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto } from "../../models/department.model";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";
import { DepartmentManagerDialogComponent } from "./department-manager-dialog/department-manager-dialog.component";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: './account-department.component.html',
})
export class AccountDepartmentComponent {      
      private dialog = inject(MatDialog);
      private departmentFacade = inject(DepartmentFacade);

      department$ = this.departmentFacade.department$;
      trackById(index: number, item: DepartmentDto) { return item.id; }

      openDeparmentRequestDialog(): void {
            const dialog = this.dialog.open(DepartmentRequestDialog);
            dialog.afterClosed().subscribe();
      }

      addDepartmentManager(dept: DepartmentDto): void {
            console.log("dept: ", dept);
            const dialogRef = this.dialog.open(DepartmentManagerDialogComponent, {
                  data: dept
            })
      }

      editDepartment(department: DepartmentDto): void {
            const dialogRef = this.dialog.open(DepartmentRequestDialog, {
                  data: department
            });            
      }

      buildDepartmentActions(dept: DepartmentDto) : ActionMenuOption[] {
            return [
                  { label: '👨🏻‍💼 Quản lý', action: () => this.addDepartmentManager(dept) },
                  { label: '⚙️ Sửa', action: () => this.editDepartment(dept) },
                  { label: '⛔ Xóa', color: 'red' },
            ]
      }
}