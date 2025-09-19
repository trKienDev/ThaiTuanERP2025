import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "./department-request/department-request.component";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto } from "../../models/department.model";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule ],
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

      editDepartment(department: DepartmentDto): void {
            const dialogRef = this.dialog.open(DepartmentRequestDialog, {
                  data: department
            });            
      }
}