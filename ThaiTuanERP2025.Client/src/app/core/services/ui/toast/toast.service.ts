import { inject, Injectable } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material/snack-bar";
import { ToastComponent } from "./toast.component";

export type ToastType = 'success' | 'error' | 'warning' | 'info';

@Injectable({ providedIn: 'root' })
export class ToastService {
      private snackbar = inject(MatSnackBar);

      private baseConfig: MatSnackBarConfig = {
            horizontalPosition: 'end',
            verticalPosition: 'top',
            panelClass: ['kit-snack']           
      };

      /** Snackbar đơn giản (text 1 dòng) */
      open(message: string, type: ToastType = 'info', cfg?: MatSnackBarConfig) {
            const panelClass = this.mergePanel(cfg, type);
            const finalCfg = this.enforceDuration({ ...this.baseConfig, ...cfg, panelClass });
            this.snackbar.open(message, 'Đóng', finalCfg);
      }

      /** Snackbar “rich”: icon + title + message */
      openRich(type: ToastType, message: string, title?: string, cfg?: MatSnackBarConfig) {
            const panelClass = this.mergePanel(cfg, type);
            const finalCfg = this.enforceDuration({ ...this.baseConfig, ...cfg, panelClass });            
            const durationMs = finalCfg.duration; // duration sau cùng (nếu đang sticky thì sẽ là undefined)
            this.snackbar.openFromComponent(ToastComponent, { ...finalCfg, data: { type, title, message, durationMs } });
      }

      // Short-hands
      success(msg: string, cfg?: MatSnackBarConfig) { this.open(msg, 'success', this.withDefaultDuration(cfg, 2500)); }
      error(msg: string,   cfg?: MatSnackBarConfig) { this.open(msg, 'error',   this.withDefaultDuration(cfg, 5000)); }
      warning(msg: string, cfg?: MatSnackBarConfig) { this.open(msg, 'warning', this.withDefaultDuration(cfg, 4000)); }
      info(msg: string,    cfg?: MatSnackBarConfig) { this.open(msg, 'info',    this.withDefaultDuration(cfg, 3000)); }

      successRich(msg: string, title = 'Thành công', cfg?: MatSnackBarConfig) { this.openRich('success', msg, title, this.withDefaultDuration(cfg, 2500)); }
      errorRich(msg: string | string[], title = 'Lỗi', cfg?: MatSnackBarConfig) {
            const text = Array.isArray(msg) ? msg.join('\n• ') : msg;
            this.openRich('error', text, title, this.withDefaultDuration(cfg, 5000));
      }
      warningRich(msg: string, title = 'Cảnh báo', cfg?: MatSnackBarConfig) { this.openRich('warning', msg, title, this.withDefaultDuration(cfg, 4000)); }
      infoRich(msg: string, title = 'Thông báo',   cfg?: MatSnackBarConfig) { this.openRich('info',    msg, title, this.withDefaultDuration(cfg, 3000)); }

      /** Gộp panelClass và gắn class theo loại (khớp SCSS: .kit-snack--success/error/...) */
      private mergePanel(cfg: MatSnackBarConfig | undefined, type: ToastType): string[] {
            const base = (cfg?.panelClass ?? this.baseConfig.panelClass ?? []) as string[] | string;
            const arr = Array.isArray(base) ? base.slice() : [String(base)];
            if (!arr.includes('kit-snack')) arr.push('kit-snack');
            const typeClass = `kit-snack--${type}`;
            if (!arr.includes(typeClass)) arr.push(typeClass);
            return arr;
      }

      /** Nếu đang sticky thì bỏ duration; nếu không thì giữ nguyên cấu hình */
      private enforceDuration(cfg: MatSnackBarConfig): MatSnackBarConfig {
            return cfg;
      }

      /** Thêm duration mặc định cho shorthand khi KHÔNG sticky */
      private withDefaultDuration(cfg: MatSnackBarConfig | undefined, ms: number): MatSnackBarConfig | undefined {
            const duration = cfg?.duration ?? ms;
            return { ...cfg, duration };
      }
}
