import { CommonModule } from "@angular/common";
import { Component, effect, inject } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { usePaymentDetail } from "../../../composables/use-payment-detail";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { OutgoingBankAccountOptionStore } from "../../../options/outgoing-bank-account-option.store";
import { KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { FileService } from "../../../../../shared/services/file.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";

type UploadStatus = 'queued' | 'uploading' | 'done' | 'error';
type UploadItem = {
      file: File;
      name: string;
      size: number;
      progress: number;    // 0..100
      status: UploadStatus;
      objectKey?: string;  // trả về từ server khi thành công
      fileId?: string;
      url?: string;
};



@Component({
      selector: 'outgoing-payment-request',
      templateUrl: './outgoing-payment-request.component.html',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent],
      styleUrls: ['./outgoing-payment-request.component.scss']
})
export class OutgoingPaymentRequestComponent {
      private route = inject(ActivatedRoute);
      private formBuilder = inject(FormBuilder);
      public submitting = false;
      private OBAccountOptionsStore = inject(OutgoingBankAccountOptionStore);
      OBAccountOptions = this.OBAccountOptionsStore.options$;
      private fileService = inject(FileService);

      private paymentLogic = usePaymentDetail();
      loading = this.paymentLogic.isLoading;
      err = this.paymentLogic.error;
      paymentDetail = this.paymentLogic.paymentDetail;

      private toast = inject(ToastService);

      private readonly uploadMeta = {
            module: 'expense',
            entity: 'outgoing-payment-attachment',
            entityId: undefined as string | undefined,
            isPublic: false
      }

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            description: this.formBuilder.control<string>(''),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            beneficiaryName: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            totalAmount: this.formBuilder.nonNullable.control<number>(0),
            totalTax: this.formBuilder.nonNullable.control<number>(0),
            totalWithTax: this.formBuilder.nonNullable.control<number>(0),
            totalOutgoing: this.formBuilder.nonNullable.control<number>(0),
      });

      uploads: UploadItem[] = [];
      
      private autoPatchEffect = effect(() => {
            const isLoading = this.loading();
            const detail = this.paymentDetail();

            // 🔹 Khi đang loading → disable form để tránh user nhập
            if (isLoading) {
                  this.form.disable({ emitEvent: false });
                  return;
            }

            // 🔹 Khi load xong (hết loading)
            this.form.enable({ emitEvent: false });

            // Nếu có dữ liệu thì patch form
            if (detail) {
                  this.form.patchValue({
                        name: `[OUT] ${detail.name}`,
                        bankName: detail.bankName,
                        accountNumber: detail.accountNumber,
                        beneficiaryName: detail.beneficiaryName,
                        totalAmount: detail.totalAmount,
                        totalTax: detail.totalTax,
                        totalWithTax: detail.totalWithTax,
                  });
            }
      });

      ngOnInit() {
            const id = this.route.snapshot.paramMap.get('id');
            if (id) this.paymentLogic.load(id);
      }

      refresh() {
            this.paymentLogic.refresh();
      }

      onFileSelected(event: Event): void {
            const input = event.target as HTMLInputElement;
            const files = Array.from((event.target as HTMLInputElement).files ?? []);
            if(!files.length) return;

            let invalidCount = 0;

            // tạo item và upload từng file
            for (const f of files) {
                  if(!f.size || f.size <= 0) {
                        invalidCount++;
                        this.toast.errorRich('File không hợp lệ');
                        continue;
                  }

                  const item: UploadItem = {
                        file: f,
                        name: f.name,
                        size: f.size,
                        progress: 0,
                        status: 'queued'
                  };
                  this.uploads.push(item);
                  this.uploadOne(item);
            }

            // reset input để có thể chọn lại cùng tên file lần sau
            (event.target as HTMLInputElement).value = '';
            input.value = '';
      }

      private uploadOne(item: UploadItem): void {
            // có nơi khác push UploadItem thẳng vào this.uploads, thêm guard đầu hàm:
            if (!item.size || item.size <= 0) {
                  item.status = 'error';
                  this.toast.errorRich('file không hợp lệ', { sticky: true });
                  return;
            }
            
            item.status = 'uploading';
            this.fileService.uploadFileWithProgress$(item.file, this.uploadMeta).subscribe({
                  next: (evt) => {
                        if(evt.type === 'progress') {
                              item.progress = Math.min(100, Math.max(0, Math.round(evt.percent)));
                        } else if(evt.type === 'done') {
                              item.progress = 100;
                              setTimeout(() => item.status = 'done', 400);
                              const data = evt.data; // UploadFileResult | undefined
                              // map kết quả tuỳ cấu trúc UploadFileResult của bạn
                              item.objectKey = data?.objectKey ?? data?.objectKey ?? data?.id ?? item.objectKey;
                              (item as any).fileId = (data as any)?.id ?? (item as any).fileId;
                              (item as any).url = (data as any)?.url ?? (item as any).url;

                              item.progress = 100;
                              item.status = 'done';
                              this.toast.successRich('Tải tệp thành công');
                        }
                  },
                  error: (err) => {
                        console.error('Upload error: ', err);
                        item.status = 'error';
                        this.toast.errorRich('Up file thất bại');
                  }
            });
      }

      removeUpload(index: number): void {
            const item = this.uploads[index];
            if(item.status === 'uploading') return;
            const fileId = item.fileId;
            if(fileId) {
                  this.fileService.hardDelete$(fileId).subscribe({
                        next: () => this.toast.successRich('Đã xóa tệp'),
                        error:(err) => {
                              const msg = handleHttpError(err);
                              const message = Array.isArray(msg) ? msg.join('\n') : String(msg);
                              this.toast.errorRich('Không xóa được tệp')
                        } 
                  });

            }
            this.uploads.splice(index, 1);
      }
}