import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { ToastService } from '../kit-toast-alert/kit-toast-alert.service';
import { UploadMeta, UploadItem } from './upload-item.model';

@Component({
      selector: 'kit-file-uploader',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-file-uploader.component.html',
      styleUrls: ['./kit-file-uploader.component.scss']
})
export class KitFileUploaderComponent {
      @Input() accept = 'application/pdf,image/*';
      @Input() multiple = true;
      @Input({ required: true }) meta!: UploadMeta;
      @Input() uploads: UploadItem[] = [];
      @Input() compact: boolean = false;

      @Output() uploadsChange = new EventEmitter<UploadItem[]>();
      @Output() completed = new EventEmitter<UploadItem>();      // bắn từng file khi done
      @Output() removed = new EventEmitter<UploadItem>();        // bắn khi xoá
      @Output() previewRequested = new EventEmitter<File>();

      private readonly toast = inject(ToastService);

      trackById(index: number, item: UploadItem) {
            return item.name;
      }

      @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
      openFilePicker() {
            if (this.fileInput) {
                  this.fileInput.nativeElement.click();
            }
      }

      onFileSelected(ev: Event) {
            const input = ev.target as HTMLInputElement;
            const files = Array.from(input.files ?? []);

            if (!files.length) return;

            // --- Giới hạn số file chọn trong 1 lần ---
            if (files.length > 10) {
                  this.toast.errorRich('Bạn chỉ được chọn tối đa 10 tệp mỗi lần tải lên');
                  input.value = '';
                  return;
            }

            // --- Giới hạn tổng số file đã upload ---
            if ((this.uploads.length + files.length) > 10) {
                  this.toast.errorRich('Tổng số tệp tối đa là 10');
                  input.value = '';
                  return;
            }

            const MAX_SIZE = 50 * 1024 * 1024; // 10MB
            for (const f of files) {
                  if (f.size > MAX_SIZE) {
                        this.toast.errorRich(`Tệp "${f.name}" vượt quá 50 MB và sẽ không được tải lên.`);
                        continue;
                  }                                                                                                                                               


                  if (!f.size) { 
                        this.toast.errorRich('File không hợp lệ'); 
                        continue; 
                  }
                  const item: UploadItem = { file: f, name: f.name, size: f.size, progress: 0, status: 'queued' };
                  this.uploads.push(item);
            }
            this.uploadsChange.emit(this.uploads);
            input.value = '';
      }

      preview(item: UploadItem) {
            this.previewRequested.emit(item.file);
      }

      remove(i: number) {
            const item = this.uploads[i];
            this.uploads.splice(i, 1);

            this.removed.emit(item);
            this.uploadsChange.emit(this.uploads);
      }

      get hasQueued() { return this.uploads.some(u => u.status === 'queued'); }
      get isFull(): boolean { return this.uploads.length >= 10; }
}
