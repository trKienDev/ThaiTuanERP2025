import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { FileService } from '../../services/file.service';
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

      private readonly fileService = inject(FileService);
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

            for (const f of files) {
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

      remove(i: number) {
            const item = this.uploads[i];
            this.uploads.splice(i, 1);

            this.removed.emit(item);
            this.uploadsChange.emit(this.uploads);
      }

      get hasQueued() { return this.uploads.some(u => u.status === 'queued'); }
}
