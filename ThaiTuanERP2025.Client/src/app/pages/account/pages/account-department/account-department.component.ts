import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "./department-request/department-request.component";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentDto } from "../../models/department.model";
import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule, OverlayModule ],
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

      openDepartmentMenu: number | null = null;
      departmentMenuOpenIndex: number | null = null;
      departmentMenuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top',    overlayX: 'end',    overlayY: 'bottom', offsetY: -8 },
      ]
      toggleDepartmentMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.departmentMenuOpenIndex = ( this.departmentMenuOpenIndex === i ) ? null : i;
      }
      onDepartmentMenuClosed() {
            this.departmentMenuOpenIndex = null;
      }
}