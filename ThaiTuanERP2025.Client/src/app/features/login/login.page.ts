import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service.js';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { catchError, finalize, tap } from 'rxjs';
import { FieldErrorComponent } from '../../shared/components/messages/field-error.component.js';
import { AlertMesssageComponent } from '../../shared/components/messages/alert-message.component.js';
import { handleApiResponse$ } from '../../shared/operators/handle-api-response.operator.js';
import { LoginResponseDto } from './login-response.model.js';
import { ToastService } from '../../shared/components/toast/toast.service.js';

@Component({
      selector: 'app-login',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, FieldErrorComponent, AlertMesssageComponent ],
      templateUrl: './login.page.html',
      styleUrls: ['./login.page.scss'],
})
export class LoginComponent implements OnInit{
      loginForm!: FormGroup;
      showPassword = false;
      message: string | null = null;
      traceId: string | null = null;
      isLoading = false;
      submitted = false;
      private toast = inject(ToastService);

      alertClosed = {
            employeeCode: false,
            password: false,
            global: false
      }

      constructor(
            private authService: AuthService, 
            private fb: FormBuilder,
            private router: Router) {}

      ngOnInit() {
            this.loginForm = this.fb.group({
                  employeeCode: ['', Validators.required],
                  password: ['', Validators.required]
            });

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

      closeAlert(field: 'employeeCode' | 'password' | 'global') {
            this.alertClosed[field] = true;
      }

      togglePassword() {
            this.showPassword = !this.showPassword;
      }

      onSubmit(): void {
            this.submitted = true;

            // Reset trạng thái đóng alert khi submit lại
            this.alertClosed = { employeeCode: false, password: false, global: false };

            if(this.loginForm.invalid) {
                  this.loginForm.markAllAsTouched();
                  return;
            }

            const { employeeCode, password } = this.loginForm.value;
            this.message = null;
            this.isLoading = true;

            this.authService.login(employeeCode, password).pipe(
                  handleApiResponse$<LoginResponseDto>(),
                  tap(response => this.authService.loginSuccess(response)),
                  catchError(err => {
                        this.toast.errorRich('Sai tài khoản hoặc mật khẩu');
                        console.error('error: ', err);
                        throw err;
                  }),
                  finalize(() => (this.isLoading = false))
            ).subscribe({
                  next: () => this.router.navigateByUrl('/splash')
            });
      };
}