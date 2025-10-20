import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FileService } from '../../services/file.service';
import { ToastService } from '../toast/toast.service';
import { UploadMeta, UploadItem, UploadFileResult } from './upload-item.model';

type UploadEvent = | { type: 'progress'; percent: number } | { type: 'done'; data?: UploadFileResult };

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
      @Output() uploadsChange = new EventEmitter<UploadItem[]>();
      @Output() completed = new EventEmitter<UploadItem>();      // bắn từng file khi done
      @Output() removed = new EventEmitter<UploadItem>();        // bắn khi xoá

      constructor(private fileService: FileService, private toast: ToastService) {}

      onFileSelected(ev: Event) {
            const input = ev.target as HTMLInputElement;
            const files = Array.from(input.files ?? []);
            if (!files.length) return;

            for (const f of files) {
                  if (!f.size) { 
                        this.toast.errorRich('File không hợp lệ'); 
                        continue; 
                  }
                  const item: UploadItem = { file: f, name: f.name, size: f.size, progress: 0, status: 'queued' };
                  this.uploads.push(item);
                  this.uploadOne(item);
            }
            this.uploadsChange.emit(this.uploads);
            input.value = '';
      }

      private uploadOne(item: UploadItem) {
            if (!item.size) { 
                  item.status = 'error'; 
                  this.toast.errorRich('File không hợp lệ', { sticky: true });
                  return;
            }

            item.status = 'uploading';
            this.fileService.uploadFileWithProgress$(item.file, this.meta).subscribe({
                  next: (evt: UploadEvent) => {
                        if (evt.type === 'progress') {
                              item.progress = Math.min(100, Math.max(0, Math.round(evt.percent)));
                        } else if (evt.type === 'done') {
                              const data = evt.data; // UploadFileResult | undefined
                              item.objectKey = data?.objectKey ?? data?.id ?? item.objectKey;
                              (item as any).fileId = data?.id ?? (item as any).fileId;
                              (item as any).url = data?.url ?? (item as any).url;

                              item.progress = 100;
                              item.status = 'done';
                              this.completed.emit(item);
                              this.uploadsChange.emit(this.uploads);
                        }
                  },
                  error: (err) => { 
                        item.status = 'error';
                        this.toast.errorRich('Tải tệp thất bại');
                  }
            });
      }

      remove(i: number) {
            const item = this.uploads[i];
            if (item.status === 'uploading') return;
            const fileId = (item as any).fileId as string | undefined;
            if (fileId) {
                  this.fileService.hardDelete$(fileId).subscribe({
                        error: () => this.toast.errorRich('Không xóa được tệp')
                  });
            }
            this.uploads.splice(i, 1);
            this.removed.emit(item);
            this.uploadsChange.emit(this.uploads);
      }

      get isUploading() { return this.uploads?.some(u => u.status === 'uploading'); }
}
