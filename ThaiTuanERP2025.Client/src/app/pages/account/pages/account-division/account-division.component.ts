import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CreateDivisionRequestComponent } from "./create-division-request/create-division-request.component";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { DivisionService } from "../../services/division.service";
import { DivisionSummaryDto } from "../../models/division.model";
import { UserService } from "../../services/user.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";

@Component({
      selector: 'account-division',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './account-division.component.html',
})
export class AccountDivisionComponent implements OnInit {
      private dialog = inject(MatDialog);
      private toastService = inject(ToastService);
      private divisionService = inject(DivisionService);
      private userService = inject(UserService);

      divisionDtos: DivisionSummaryDto[] = [];

      ngOnInit(): void {
            this.loadDivisions();
      }

      loadDivisions(): void {
            this.divisionService.getAll().subscribe({
                  next: (divisions) => {
                        console.log('divisions: ', divisions);
                        this.divisionDtos = divisions;
                  },
                  error: (err) => {
                        const messages = handleHttpError(err).join('\n');
                        this.toastService.errorRich(messages || 'Tải danh sách khối thất bại');
                  }
            })
      }

      openCreateDivisionRequest(): void {
            const dialogRef = this.dialog.open(CreateDivisionRequestComponent, {
                  width: 'fit-content',
                  height: 'fit-content',
                  maxWidth: '90vw',   
                  maxHeight: '80vh',
                  disableClose: true,
            });

            dialogRef.afterClosed().subscribe((result: { isSuccess: boolean}) => {
                  if(result.isSuccess == true) {
                        this.loadDivisions();
                        this.toastService.successRich('Thêm khối thành công');
                  }
            })
      }
}