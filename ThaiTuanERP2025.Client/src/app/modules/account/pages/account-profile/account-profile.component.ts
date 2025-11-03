import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { environment } from "../../../../../environments/environment";
import { UserDto } from "../../models/user.model";
import { UserFacade } from "../../facades/user.facade";
import { firstValueFrom } from "rxjs";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { UserService } from "../../services/user.service";
import { AvatarUrlPipe } from "../../../../shared/pipes/avatar-url.pipe";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'account-profile',
      standalone: true,
      imports: [CommonModule, MatSnackBarModule, AvatarUrlPipe, KitSpinnerButtonComponent],
      templateUrl: './account-profile.component.html',
      styleUrl: './account-profile.component.scss',
})
export class AccountProfileComponent implements OnInit {
      private userFacade = inject(UserFacade);
      private toastService = inject(ToastService);
      private userService = inject(UserService);

      baseUrl: string = environment.baseUrl;      
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      selectedAvatarFile: File | null = null;
      isUploading: boolean = false;

      previewAvatarSrc: string | null = null;
      
      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);
      }

      triggerAvatarUpload(): void {
            const fileInput = document.getElementById('avatar-input') as HTMLInputElement;
            if (fileInput) {
                  fileInput.value = '';
                  fileInput.click();
            }
      }

      onAvatarSelected(event: Event): void {
            const input = event.target as HTMLInputElement;
            if (!input.files || input.files.length === 0) return;

            const file = input.files[0];
            const allowedTypes = ['image/jpg', 'image/jpeg', 'image/png'];

            if (!allowedTypes.includes(file.type)) {
                  this.toastService.errorRich('Chỉ hỗ trợ ảnh .JPEG, .JPG hoặc .PNG', 'Đóng');
                  return;
            }

            this.selectedAvatarFile = file;

            const reader = new FileReader();
            reader.onload = () => {
                  this.previewAvatarSrc = reader.result as string;
            };
            reader.readAsDataURL(file);
      }
            
      async uploadAvatar(): Promise<void> {
            if (!this.selectedAvatarFile || !this.currentUser?.id) {
                  this.toastService.errorRich('Thiếu thông tin người dùng hoặc file');
                  return;
            }

            this.isUploading = true;
            try {
                  await firstValueFrom(this.userService.updateAvatar(this.selectedAvatarFile, this.currentUser.id));
                  await this.userFacade.refreshCurrentUser();
                  this.currentUser = await firstValueFrom(this.currentUser$);
                  this.toastService.successRich('Cập nhật avatar thành công');
            } catch (err) {
                  console.error(err);
                  this.toastService.errorRich('Không thể cập nhật ảnh đại diện');
            } finally {
                  this.isUploading = false;
            }
      }
}