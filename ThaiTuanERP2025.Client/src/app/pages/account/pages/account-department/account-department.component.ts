import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { DepartmentService } from "../../services/department.service";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { KitDropdownOption} from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { MatDialog } from "@angular/material/dialog";
import { DepartmentRequestDialog } from "./department-request/department-request.component";

@Component({
      selector: 'account-department',
      standalone: true,
      imports: [CommonModule ],
      templateUrl: './account-department.component.html',
})
export class AccountDepartmentComponent implements OnInit {      
      private toastService = inject(ToastService);
      private dialog = inject(MatDialog);

      userOptions: KitDropdownOption[] = [];
      
      ngOnInit(): void {

      }

      openDeparmentRequestDialog(): void {
            const dialog = this.dialog.open(DepartmentRequestDialog);
            dialog.afterClosed().subscribe((result?: { isSuccess?: boolean }) => {

            });
      }

}