import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { User } from "../../models/user.model";
import { UserService } from "../../services/user.service";
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { environment } from "../../../../../environments/environment";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule, MatSnackBarModule ],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent implements OnInit {
      baseUrl: string = environment.baseUrl;
      user: User | null = null;
      selectedAvatarFile: File | null = null;
      isUploading: boolean = false;
      
      constructor(private userService: UserService, private snackBar: MatSnackBar) {}

      ngOnInit(): void {
            this.userService.getCurrentuser().subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.user = res.data;
                        }
                  },
                  error: () => {
                        alert('Không thể lấy thông tin người dùng hiện tại');
                  }
            });
      }

      triggerAvatarUpload(): void {
            const fileInput = document.getElementById('avatar-input') as HTMLInputElement;
            fileInput?.click();
      }

      get avatarSrc(): string {
            if (this.user?.avatarUrl) {
                  if (this.user.avatarUrl.startsWith('data:image')) {
                        return this.user.avatarUrl; // base64 → dùng trực tiếp
                  }
                  return this.baseUrl + this.user.avatarUrl; // server path
            }
            return 'assets/default-user-avatar.jpg';
      }
      onAvatarSelected(event: Event): void {
            const input = event.target as HTMLInputElement;
            if(input.files && input.files.length > 0) {
                  const file = input.files[0];

                  const allowedTypes = ['image/jpg', 'image/jpeg', 'image/png'];
                  if(!allowedTypes.includes(file.type)) {
                        alert('Chỉ nhận ảnh .JPEG, .JPG hoặc .PNG');
                        return;
                  }

                  this.selectedAvatarFile = file;
                  const reader = new FileReader();
                  reader.onload = () => {
                        if(this.user) {
                              this.user.avatarUrl = reader.result as string;
                        }
                  };
                  reader.readAsDataURL(file); 
            }
      }
      uploadAvatar(): void {
            if(!this.selectedAvatarFile) {
                  this.snackBar.open('Vui lòng chọn avatar trước', 'Đóng', { duration: 3000 });
                  return;
            }

            this.isUploading = true;

            this.userService.updateAvatar(this.selectedAvatarFile).subscribe({
                  next: (res) => {
                        this.isUploading = false;
                        if(res.isSuccess && res.data) {
                              if(this.user) {
                                    this.user.avatarUrl = res.data;
                              }
                              this.selectedAvatarFile = null;
                              this.snackBar.open('Cập nhật ảnh đại diện thành công', 'Đóng', { duration: 3000 });
                        } else {
                              this.snackBar.open(res.message || 'Cập nhật ảnh thất bại', 'Đóng', { duration: 3000 });
                        }
                  }, 
                  error: (err) => {
                         this.isUploading = false;
                        console.error('Upload avatar error: ', err);
                         this.snackBar.open('Không thể cập nhật ảnh đại diện', 'Đóng', { duration: 3000 });
                  }
            })
      }
}