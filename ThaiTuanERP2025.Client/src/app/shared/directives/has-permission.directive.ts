import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';

@Directive({
      selector: '[appHasPermission]',
      standalone: true
})
export class HasPermissionDirective {
      private permission = '';

      constructor(
            private templateRef: TemplateRef<any>,
            private viewContainer: ViewContainerRef,
            private authService: AuthService
      ) {}

      @Input() set appHasPermission(permission: string) {
            this.permission = permission;
            this.updateView();
      }

      private updateView(): void {
            if (this.authService.hasPermission(this.permission)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
            } else {
                  this.viewContainer.clear();
            }
      }
}
