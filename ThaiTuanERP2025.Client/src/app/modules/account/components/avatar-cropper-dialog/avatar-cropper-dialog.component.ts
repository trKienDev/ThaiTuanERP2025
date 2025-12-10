import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ImageCroppedEvent, ImageCropperComponent } from "ngx-image-cropper";
import { UserApiService } from "../../services/api/user-api.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { UserFacade } from "../../facades/user.facade";
import { firstValueFrom } from "rxjs";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";

@Component({
      selector: 'avatar-cropper-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, ImageCropperComponent],
      templateUrl: './avatar-cropper-dialog.component.html',
      styleUrls: ['./avatar-cropper-dialog.component.scss']
})
export class AvatarCropperDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<AvatarCropperDialogComponent>);
      private readonly data = inject(MAT_DIALOG_DATA);
      private readonly userApi = inject(UserApiService);
      private readonly toast = inject(ToastService);
      private readonly userFacade = inject(UserFacade);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      imageChangedEvent!: Event;
      croppedBlob: Blob | null = null;
      isUploading = false;

      constructor() {
            this.imageChangedEvent = this.data.imageEvent; // nhận từ parent
      }

      onImageCropped(event: ImageCroppedEvent) {
            this.croppedBlob = event.blob ?? null;
      }

      async save() {
            if (!this.croppedBlob) {
                  this.toast.warningRich('Không có hình ảnh hợp lệ');
                  return;
            }

            const userId = this.data.userId;
            const file = new File([this.croppedBlob], 'avatar.png', { type: 'image/png' });

            this.isUploading = true;

            try {
                  await firstValueFrom(this.userApi.updateAvatar(file, userId));

                  await this.userFacade.refreshCurrentUser();  // reload profile
                  this.toast.successRich('Cập nhật avatar thành công');

                  this.dialogRef.close(true);                 
            } catch (err) {
                  this.httpErrorHandler.handle(err, "Có lỗi khi cập nhật avatar");
            } finally {
                  this.isUploading = false;
            }
      }

      close() {
            this.dialogRef.close(false);
      }
}