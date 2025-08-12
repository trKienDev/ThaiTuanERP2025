import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from '../../core/models/api-response.model.js';
import { LoginResponse } from '../../core/models/login-response.model.js';
import { AuthService } from '../../core/services/auth/auth.service.js';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { handleApiResponse } from '../../core/utils/handle-api-response.utils.js';
import { handleApiResponse$ } from '../../core/utils/handle-api-response.operator.js';
import { catchError, EMPTY, finalize, tap } from 'rxjs';
import { handleHttpError } from '../../core/utils/handle-http-errors.util.js';

@Component({
      selector: 'app-login',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule ],
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
                  handleApiResponse$<LoginResponse>(),
                  tap((data) => this.authService.loginSuccess(data.accessToken, data.userRole)),
                  catchError((err) => {
                        const msgs = handleHttpError(err);
                        this.message = msgs[0];
                        return EMPTY; // nuốt lỗi để subscribe.next không chạy, và không gọi error callback
                  }),
                  finalize(() => (this.isLoading = false))
            ).subscribe({
                  next: () => {
                        this.router.navigateByUrl('/splash')
                  }
            });
      };
}
