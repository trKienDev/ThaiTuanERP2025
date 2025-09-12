import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { UserService } from "../../services/user.service";
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { environment } from "../../../../../environments/environment";
import { UserDto } from "../../models/user.model";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule, MatSnackBarModule ],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent implements OnInit {
      baseUrl: string = environment.baseUrl;
      user: UserDto | null = null;
      selectedAvatarFile: File | null = null;
      isUploading: boolean = false;
      
      constructor(private userService: UserService, private snackBar: MatSnackBar) {}

      ngOnInit(): void {
            console.log('url', this.baseUrl);
            this.loadCurrentUser();
      }

      private loadCurrentUser(): void {
            this.userService.getCurrentuser().subscribe({
                  next: (data) => this.user = data,
                  error: () => this.snackBar.open('Không thể lấy thông tin người dùng', 'Đóng', { duration: 3000 }),
            });
      }

      triggerAvatarUpload(): void {
            const fileInput = document.getElementById('avatar-input') as HTMLInputElement;
            fileInput?.click();
      }

      get avatarSrc(): string {
            if (this.user?.avatarFileObjectKey) {
                  return this.baseUrl + '/files/public/' + this.user.avatarFileObjectKey;
            }
            return 'default-user-avatar.jpg';
      }
      onAvatarSelected(event: Event): void {
            const input = event.target as HTMLInputElement;
            if(input.files && input.files.length > 0) {
                  const file = input.files[0];

                  const allowedTypes = ['image/jpg', 'image/jpeg', 'image/png'];
                  if(!allowedTypes.includes(file.type)) {
                        this.snackBar.open('Chỉ hỗ trợ ảnh .JPEG, .JPG hoặc .PNG', 'Đóng', { duration: 3000 });
                        return;
                  }

                  this.selectedAvatarFile = file;

                  // preview ảnh bằng base64
                  const reader = new FileReader();
                  reader.onload = () => {
                        if(this.user) {
                              this.user.avatarFileId = reader.result as string;
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

            if(!this.user) 
                  return;

            this.isUploading = true;
            if (!this.user || !this.user.id) {
                  this.snackBar.open('Không xác định được ID người dùng', 'Đóng', { duration: 3000 });
                  return;
            }

            this.userService.updateAvatar(this.selectedAvatarFile, this.user.id).subscribe({
                  next: (url) => {
                        this.user!.avatarFileId = url; // gán đường dẫn mới trả về
                        this.selectedAvatarFile = null;
                        this.snackBar.open('Cập nhật avatar thành công', 'Đóng', { duration: 3000 });
                        this.isUploading = false;
                  }, 
                  error: (err) => {
                        this.isUploading = false;
                        console.error('Upload avatar error: ', err);
                        this.snackBar.open('Không thể cập nhật ảnh đại diện', 'Đóng', { duration: 3000 });
                  }
            })
      }
}