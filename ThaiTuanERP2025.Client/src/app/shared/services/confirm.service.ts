import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogData, KitConfirmDialogComponent } from "../components/confirm-dialog/confirm-dialog.component";

@Injectable({ providedIn: 'root' })
export class ConfirmService {
      constructor(private dialog: MatDialog) {}

      /** Hộp thoại xác nhận chung — trả về Promise<boolean> */
      ask(data: ConfirmDialogData): Promise<boolean> {
            return this.dialog.open(KitConfirmDialogComponent, { 
                  width: '420px', data, disableClose: true
            })
            .afterClosed()
            .toPromise()
            .then(res => !!res);
      }

      /** Preset: cảnh báo (màu warn) */
      warn(message: string, title = 'Xác nhận', confirmText = 'Đồng ý', cancelText = 'Hủy'): Promise<boolean> {
            return this.ask({ title, message, confirmText, cancelText, tone: 'warning' });
      }

      /** Preset: thao tác nguy hiểm (màu warn/đỏ) */
      danger(message: string, title = 'Cảnh báo', confirmText = 'Tiếp tục', cancelText = 'Hủy'): Promise<boolean> {
            return this.ask({ title, message, confirmText, cancelText, tone: 'danger' });
      }

      /** Tiện ích: hỏi thay thế hoá đơn cho 1 dòng items */
      async confirmReplaceInvoice(alreadyHasInvoice: boolean): Promise<boolean> {
      if (!alreadyHasInvoice) return true; // không có invoice thì cho qua
            return this.warn('Dòng này đã gắn một hóa đơn. Bạn có muốn thay thế không?', 'Thay thế hóa đơn', 'Thay thế', 'Giữ nguyên');
      }
}