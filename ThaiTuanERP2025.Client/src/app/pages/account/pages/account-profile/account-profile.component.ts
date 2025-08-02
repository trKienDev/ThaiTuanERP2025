import { CommonModule } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { User } from "../../models/user.model";
import { UserService } from "../../services/user.service";
import { read } from "xlsx";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule ],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent implements OnInit {
      user: User | null = null;
      
      constructor(private userService: UserService) {}

      ngOnInit(): void {
            this.userService.getCurrentuser().subscribe({
                  next: (res) => {
                        if(res.isSuccess && res.data) {
                              this.user = res.data;
                              console.log('this.user: ', this.user);
                              console.log('user avatar: ', this.user?.avatarUrl);
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

      onAvatarSelected(event: Event): void {
            const input = event.target as HTMLInputElement;
            if(input.files && input.files.length > 0) {
                  const file = input.files[0];

                  const allowedTypes = ['image/jpg', 'image/jpeg', 'image/png'];
                  if(!allowedTypes.includes(file.type)) {
                        alert('Chỉ nhận ảnh .JPEG, .JPG hoặc .PNG');
                        return;
                  }

                  // Tạm thời hiện thị trước khi chọn
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
            
      }
}