import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service.js';
import { ApiResponse } from '../../core/models/api-response.model.js';
import { LoginResponse } from '../../core/models/login-response.model.js';

@Component({
      selector: 'app-login',
      standalone: true,
      imports: [CommonModule, FormsModule ],
      templateUrl: './login.page.html',
      styleUrls: ['./login.page.scss'],
})
export class LoginComponent implements OnInit{
      username = '';
      password = '';
      showPassword = false;
      message: string | null = null;
      isLoading = false;

      constructor(private authService: AuthService, private router: Router) {}

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

      login() {
            this.message = null;
            this.isLoading = true;

            this.authService.login(this.username, this.password).subscribe({
                  next: (res: ApiResponse<LoginResponse>) => {
                        this.isLoading = false;
                        
                        if(res.isSuccess && res.data?.accessToken && res.data.userRole) {
                              const { accessToken, userRole } = res.data;

                              this.authService.loginSuccess(accessToken, userRole);
                              this.router.navigateByUrl('/splash');
                        } else {
                              this.message = res.message || 'Đăng nhập thất bại';
                        }
                  },
                  error: () => {
                        this.isLoading = false;
                        this.message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau";
                  }
            });
      }
}
