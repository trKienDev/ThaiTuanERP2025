import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogData, KitConfirmDialogComponent } from "../components/confirm-dialog/confirm-dialog.component";
import { map, Observable, of } from "rxjs";

@Injectable({ providedIn: 'root' })
export class ConfirmService {
      constructor(private dialog: MatDialog) {}

      /** Hộp thoại xác nhận chung — trả về Observable<boolean> */
      ask$(data: ConfirmDialogData): Observable<boolean> {
            return this.dialog.open(KitConfirmDialogComponent, {
                  width: '420px',
                  data,
                  disableClose: true
            })
            .afterClosed()
            .pipe(map(res => !!res));
      }

      /** Preset: cảnh báo (màu warn) */
      warn$(message: string, title = 'Xác nhận', confirmText = 'Đồng ý', cancelText = 'Hủy'): Observable<boolean> {
            return this.ask$({ title, message, confirmText, cancelText, tone: 'warning' });
      }

      /** Preset: thao tác nguy hiểm (màu đỏ) */
      danger$(message: string, title = 'Cảnh báo', confirmText = 'Tiếp tục', cancelText = 'Hủy'): Observable<boolean> {
            return this.ask$({ title, message, confirmText, cancelText, tone: 'danger' });
      }

      /** Tiện ích: hỏi thay thế hoá đơn cho 1 dòng items */
      confirmReplaceInvoice$(alreadyHasInvoice: boolean): Observable<boolean> {
            if (!alreadyHasInvoice) return of(true); // không có invoice thì cho qua
            return this.warn$('Dòng này đã gắn một hóa đơn. Bạn có muốn thay thế không?', 'Thay thế hóa đơn', 'Thay thế', 'Giữ nguyên');
      }

      validateBudgetLimit$(isOverBudget: boolean): Observable<boolean> {
            if (!isOverBudget) return of(true);
            return this.danger$('Vượt ngân sách','Số tiền vượt quá ngân sách còn lại', '', 'Hủy');
      }

}