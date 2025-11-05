import { inject, Injectable } from "@angular/core";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material/snack-bar";
import { ToastComponent } from "./kit-toast-alert.component";

export type ToastType = 'success' | 'error' | 'warning' | 'info';
export interface ToastRichOptions extends MatSnackBarConfig {
      title?: string;
      sticky?: boolean;
}

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
      // toast.service.ts
      openRich(type: ToastType, message: string, title?: string, cfg?: MatSnackBarConfig & { sticky?: boolean }) {
            const panelClass = this.mergePanel(cfg, type);

            // 1) Gộp cấu hình trước
            const mergedCfg = { ...this.baseConfig, ...cfg, panelClass };

            // 2) LƯU duration lại TRƯỚC khi enforceDuration xoá nó khi sticky = true
            const durationMs = mergedCfg.duration;

            // 3) Áp dụng sticky: xoá duration nếu sticky = true (để snackbar không tự đóng)
            const finalCfg = this.enforceDuration(mergedCfg);

            // 4) Truyền durationMs vào data cho component hiển thị progress
            this.snackbar.openFromComponent(ToastComponent, {
                  ...finalCfg,
                  data: { type, title, message, durationMs }
            });
      }


      // Short-hands
      success(msg: string, cfg?: MatSnackBarConfig) { this.open(msg, 'success', this.withDefaultDuration(cfg, 2500)); }
      error(msg: string,   cfg?: MatSnackBarConfig) { this.open(msg, 'error',   this.withDefaultDuration(cfg, 5000)); }
      warning(msg: string, cfg?: MatSnackBarConfig) { this.open(msg, 'warning', this.withDefaultDuration(cfg, 4000)); }
      info(msg: string,    cfg?: MatSnackBarConfig) { this.open(msg, 'info',    this.withDefaultDuration(cfg, 3000)); }

      successRich(msg: string, titleOrOptions?: string | ToastRichOptions, cfg?: ToastRichOptions) {
            const { title, options } = this.normalizeRichArgs('Thành công', titleOrOptions, cfg);
            this.openRich('success', msg, title, this.withDefaultDuration(options, 2500));
      }

      errorRich(msg: string, titleOrOptions?: string | ToastRichOptions, cfg?: ToastRichOptions) {
            const { title, options } = this.normalizeRichArgs('Lỗi', titleOrOptions, cfg);
            this.openRich('error', msg, title, this.withDefaultDuration(options, 5000));
      }

      warningRich(msg: string, titleOrOptions?: string | ToastRichOptions, cfg?: ToastRichOptions) {
            const { title, options } = this.normalizeRichArgs('Cảnh báo', titleOrOptions, cfg);
            this.openRich('warning', msg, title, this.withDefaultDuration(options, 4000));
      }

      infoRich(msg: string, titleOrOptions?: string | ToastRichOptions, cfg?: ToastRichOptions) {
            const { title, options } = this.normalizeRichArgs('Thông báo', titleOrOptions, cfg);
            this.openRich('info', msg, title, this.withDefaultDuration(options, 3000));
      }

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
      private enforceDuration(cfg: MatSnackBarConfig & { sticky?: boolean }): MatSnackBarConfig {
            if (cfg.sticky) {
                  // không set duration => snackbar sẽ không auto-close
                  const { duration, ...rest } = cfg;
                  return rest;
            }
            return cfg;
      }

      /** Thêm duration mặc định cho shorthand khi KHÔNG sticky */
      private withDefaultDuration(cfg: MatSnackBarConfig | undefined, ms: number): MatSnackBarConfig | undefined {
            const duration = cfg?.duration ?? ms;
            return { ...cfg, duration };
      }

      /** Hỗ trợ cả kiểu cũ (title string) lẫn kiểu mới (options object) */
      private normalizeRichArgs(defaultTitle: string, titleOrOptions?: string | ToastRichOptions, cfg?: ToastRichOptions) {
            if (typeof titleOrOptions === 'string' || typeof titleOrOptions === 'undefined') {
                  return { title: titleOrOptions ?? defaultTitle, options: cfg };
            }
            // titleOrOptions là object options
            const options = titleOrOptions;
            const title = options.title ?? defaultTitle;
            return { title, options };
      }
}
