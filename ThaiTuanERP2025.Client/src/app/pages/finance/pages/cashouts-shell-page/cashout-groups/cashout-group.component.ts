import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutGroupRequestDialogComponent } from "./cashout-group-request-dialog/cashout-group-request-dialog.component";
import { CashoutGroupDto } from "../../../models/cashout-group.model";
import { CashoutGroupService } from "../../../services/cashout-group.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/toast/toast.service";

@Component({
      selector: 'cashout-group-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-group.component.html'
})
export class CashoutGroupPanelComponent implements OnInit {
      private cashoutGroups: CashoutGroupDto[] = [];
      private readonly cashoutGroupSerive = inject(CashoutGroupService);
      private readonly toastService = inject(ToastService);

      constructor(
            private dialog: MatDialog
      ) {}

      ngOnInit(): void {
            this.loadCashoutGroups();
      }

      loadCashoutGroups(): void {
            this.cashoutGroupSerive.getAll().subscribe({
                  next: (groups) => {
                        this.cashoutGroups = groups;
                  }, 
                  error: (err => {
                        const message = handleHttpError(err);
                        this.toastService.errorRich('Lỗi tải nhóm dòng tiền ra');
                  })
            })
      }

      openCashoutGroupRequestModal(): void {
            const dialogRef = this.dialog.open(CashoutGroupRequestDialogComponent, {
                  width: 'fit-content',
                  height: 'fit-content',
                  maxWidth: '90vw',
                  maxHeight: '80vh',
                  disableClose: true,
            });
      }
}