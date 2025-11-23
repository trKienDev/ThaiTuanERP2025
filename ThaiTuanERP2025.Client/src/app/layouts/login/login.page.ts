import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service.js';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { ToastService } from '../../shared/components/kit-toast-alert/kit-toast-alert.service.js';
import { KitSpinnerButtonComponent } from "../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { HttpErrorHandlerService } from '../../core/services/http-errror-handler.service.js';

@Component({
      selector: 'app-login',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent],
      templateUrl: './login.page.html',
      styleUrls: ['./login.page.scss'],
})
export class LoginComponent implements OnInit{
      showPassword = false;
      traceId: string | null = null;
      public submitting: boolean = false;
      public showErrors: boolean = false;
      private readonly toast = inject(ToastService);
      private readonly auth = inject(AuthService); 
      private readonly formBuilder = inject(FormBuilder);
      private readonly router = inject(Router);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      form = this.formBuilder.group({
            employeeCode: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]}),
            password: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]})
      });

      ngOnInit() {

            const logoWrapper = document.querySelector('.logo-wrapper');

            const triggerShine = () => {
                  if (logoWrapper) {
                        logoWrapper.classList.remove('shine-once'); // reset nếu có
                        // Kích hoạt lại animation bằng cách reflow DOM
                        void (logoWrapper as HTMLElement).offsetWidth;
                        logoWrapper.classList.add('shine-once');

                        // Xóa sau animation
                        setTimeout(() => {
                              logoWrapper?.classList.remove('shine-once');
                        }, 1600); // thời gian animation = 1.5s + 0.1s
                  }
            };

            // Chạy 1 lần khi vào trang
            setTimeout(triggerShine, 1500); // Delay 1 chút để đảm bảo DOM render xong

            // Sau đó lặp lại mỗi 5s
            setInterval(triggerShine, 10000);
      }

      togglePassword() {
            this.showPassword = !this.showPassword;
      }

      async onLogging() {
            this.showErrors = true;
            
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đầy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const { employeeCode, password } = this.form.getRawValue();
                  await firstValueFrom(this.auth.login(employeeCode, password));
                  this.toast.successRich("Xác thực thành công");
                  this.router.navigateByUrl('/splash');
                  this.showErrors = false;
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Đăng nhập thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      };
}