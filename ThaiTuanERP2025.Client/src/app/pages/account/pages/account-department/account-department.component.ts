import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "./department-request/department-request.component";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto } from "../../models/department.model";
import { DepartmentManagerDialogComponent } from "./department-manager-dialog/department-manager-dialog.component";
import { ActionMenuOption } from "../../../../shared/components/kit-action-menu/kit-action-menu.model";
import { KitActionMenuComponent } from "../../../../shared/components/kit-action-menu/kit-action-menu.component";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule, KitActionMenuComponent],
      templateUrl: './account-department.component.html',
})
export class AccountDepartmentComponent implements OnInit {      
      private dialog = inject(MatDialog);
      private departmentFacade = inject(DepartmentFacade);

      department$ = this.departmentFacade.department$;
      
      ngOnInit(): void {
            this.department$.subscribe({
                  next: (departments) =>{
                        console.log('departments: ', departments);
                  }
            })
      }

      trackById(index: number, item: DepartmentDto) { return item.id; }

      openDeparmentRequestDialog(): void {
            const dialog = this.dialog.open(DepartmentRequestDialog);
            dialog.afterClosed().subscribe();
      }

      addDepartmentManager(dept: DepartmentDto): void {
            const dialogRef = this.dialog.open(DepartmentManagerDialogComponent, {
                  data: dept
            })
      }

      editDepartment(department: DepartmentDto): void {
            const dialogRef = this.dialog.open(DepartmentRequestDialog, {
                  data: department
            });            
      }

      buildDepartmentActions(dept: DepartmentDto): ActionMenuOption[] {
            return [
                  { label: '👨🏻‍💼 Chỉnh sửa quản lý', action: () => this.addDepartmentManager(dept) },
                  { label: '⚙️ Sửa' },
                  { label: '⛔ Xóa', color: 'red' },
            ]
      }

}