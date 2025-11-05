/**
 * ThaiTuanERP2025 - Permission Directive
 * ---------------------------------------
 * Dùng để kiểm tra quyền người dùng và điều khiển hiển thị phần tử UI.
 * 
 * Hỗ trợ:
 *  - *appHasPermission="'code'" → 1 quyền cụ thể
 *  - *appHasAnyPermission="['code1', 'code2']" → có ít nhất 1 quyền
 *  - *appHasAllPermissions="['code1', 'code2']" → có tất cả quyền
 * 
 * Option:
 *  - [mode]="'disable'" → disable element thay vì ẩn
 */

import { Directive, ElementRef, Input, OnInit, Renderer2, TemplateRef, ViewContainerRef } from "@angular/core";
import { AuthService } from "./auth.service";

@Directive({
      selector: '[appHasPermission], [appHasAnyPermission], [appHasAllPermissions]',
      standalone: true
})
export class HasPermissionDirective implements OnInit {
      @Input('appHasPermission') singlePermission?: string;
      @Input('appHasAnyPermission') anyPermissions?: string[];
      @Input('appHasAllPermissions') allPermissions?: string[];
      @Input() mode: 'hide' | 'disable' = 'hide';

      constructor(
            private readonly templateRef: TemplateRef<any>,
            private readonly viewContainerRef: ViewContainerRef,
            private readonly authService: AuthService,
            private readonly renderer2: Renderer2,
            private readonly elementRef: ElementRef
      ) {
            console.log('%c[HasPermissionDirective constructed]', 'color: blue');
      }

      ngOnInit(): void {
            console.log('run HasPermissionDirective');
            const can = this.evaluatePermission();
            console.log('can: ', can);

            if (this.mode === 'hide') {
                  // === Ẩn hoặc hiển thị element ===
                  this.viewContainerRef.clear();
                  if (can) this.viewContainerRef.createEmbeddedView(this.templateRef);
            } else {
                  // === Disable element nếu không có quyền ===
                  this.viewContainerRef.createEmbeddedView(this.templateRef);
                  if (!can) {
                        const native = this.elementRef.nativeElement.nextSibling as HTMLElement;

                        if (native) {
                              this.renderer2.setAttribute(native, 'disabled', 'true');
                              this.renderer2.addClass(native, 'disabled');
                              this.renderer2.setStyle(native, 'pointer-events', 'none');
                              this.renderer2.setStyle(native, 'opacity', '0.6');
                              this.renderer2.setStyle(native, 'cursor', 'not-allowed');
                        }
                  }
            }
      }


      private evaluatePermission(): boolean {
            if (this.authService.hasRole('SuperAdmin')) {
                  return true;
            }

            if (this.singlePermission)
                  return this.authService.hasPermission(this.singlePermission);

            if (this.anyPermissions?.length)
                  return this.anyPermissions.some(p => this.authService.hasPermission(p))

            if (this.allPermissions?.length)
                  return this.allPermissions.every(p => this.authService.hasPermission(p));

            return false;
      }
}