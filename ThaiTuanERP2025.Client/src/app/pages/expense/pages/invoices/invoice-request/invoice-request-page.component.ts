import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatInputModule } from "@angular/material/input";
import { Router } from "@angular/router";
import { FileService } from "../../../../../core/services/api/file.service";
import { InvoiceService } from "../../../services/invoice.service";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import { firstValueFrom, forkJoin, lastValueFrom } from "rxjs";
import { AddInvoiceLineRequest, ReplaceMainInvoiceFileRequest } from "../../../models/invoice.model";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MoneyFormatDirective } from "../../../../../shared/directives/money/money-format.directive";

@Component({
      standalone: true,
      selector: 'expense-create-invoice',
      imports: [ CommonModule, ReactiveFormsModule, MatButtonModule, MatInputModule, MatFormFieldModule,
            MoneyFormatDirective
      ],
      templateUrl: './invoice-request-page.component.html',
      styleUrl: './invoice-request-page.component.scss',
})
export class InvoiceRequestPageComponent {
      private formBuilder = inject(FormBuilder);
      private fileService = inject(FileService);
      private invoiceService = inject(InvoiceService);
      private sanitizer = inject(DomSanitizer);
      private blobUrl?: string; // track blob để revoke

      constructor(private router: Router) {}

      totalGross = 0;
      totalVat = 0;
      totalWithVat = 0;

      pendingFile: File | null = null;

      form = this.formBuilder.group({
            invoiceNumber: ['', Validators.required],
            invoiceName: ['', Validators.required],
            issueDate: ['', Validators.required],
            paymentDate: [''],

            sellerName: [''],
            sellerTaxCode: ['', Validators.required],
            sellerAddress: [''],
            buyerName: [''],
            buyerTaxCode: [''],
            buyerAddress: [''],

            currency: ['VND'],

            mainFileId: this.formBuilder.control<string | null>(null),

            invoiceLines: this.formBuilder.array([]),

            fileIds: this.formBuilder.control<string[]>([]),

            followerUserIds: this.formBuilder.control<string[]>([]),
      });

      // ====== convenience getters ======
      get lines(): FormArray {
            return this.form.get('invoiceLines') as FormArray;
      }

      addLine() {
            const group = this.formBuilder.group({
                  itemName: ['', Validators.required],
                  unit: [''],
                  quantity: [1, [Validators.required, Validators.min(0)]],
                  unitPrice: [0, [Validators.required, Validators.min(0)]],
                  discountRate: [null],
                  discountAmount: [null],
                  gross: [{ value: 0, disabled: true }],
                  vatRatePercent: [null, [Validators.min(0), Validators.max(100)]],
                  vatAmount: [{ value: 0, disabled: true }],
                  whtTypeId: [null],
                  totalWithVat: [{ value: 0, disabled: true }],
            }, { validators: [ this.discountNotExceedGrossValidator ] });

            this.lines.push(group);
            this.bindAutoCalc(group);
            this.recomputeTotals();
      }
      removeLine(i: number) {
            this.lines.removeAt(i);
            this.recomputeTotals();
      }
      trackByIndex = (_: number, __: unknown) => _;
      private bindAutoCalc(group: FormGroup) {
            const quantityCtrl = group.get('quantity')!;
            const priceCtrl = group.get('unitPrice')!;
            const discountAmountCtrl = group.get('discountAmount')!;
            const rateCtrl = group.get('vatRatePercent')!;
            const grossCtrl = group.get('gross')!;
            const vatCtrl = group.get('vatAmount')!;
            const totalCtrl = group.get('totalWithVat')!;

            const round2 = (numb: number) => Math.round((numb + Number.EPSILON) * 100) / 100;
            const recalc = () => {
                  const quantity = Number(quantityCtrl.value) || 0;
                  const price = Number(priceCtrl?.value) || 0;
                  const rate = Number(rateCtrl.value) || 0;
                  const gross = round2(quantity * price);

                  let discount = Number(discountAmountCtrl.value) || 0;
                  if(discount < 0) discount = 0;
                  if(discount > gross) discount = gross;

                  const net = round2(gross - discount);
                  const vat = round2(net * (rate / 100));
                  const total = round2(net + vat);

                  // setValue với emitEvent:false để tránh vòng lặp
                  grossCtrl.setValue(gross, { emitEvent: false });
                  vatCtrl.setValue(vat, { emitEvent: false }),
                  totalCtrl.setValue(total, { emitEvent: false });

                  this.recomputeTotals(); // Cập nhật tổng
            };

            // tính lần đầu
            recalc();

            // lắng nghe thay đổi
            quantityCtrl.valueChanges.subscribe(recalc);
            priceCtrl.valueChanges.subscribe(recalc);
            rateCtrl.valueChanges.subscribe(recalc);
            discountAmountCtrl.valueChanges.subscribe(recalc);
      }

      // === Upload & Preview main file ===
      mainFileName: string | null = null;
      previewUrl: SafeResourceUrl | null = null; // blob URL
      previewKind: "pdf" | "image" | "other" = "other";

      async onMainFileSelected(ev: Event) {
            const input = ev.target as HTMLInputElement;
            const file = input.files?.[0];
            if(!file) return;

            // cleanup blob cũ nếu có
            if(this.blobUrl?.startsWith('blob:')) 
                  URL.revokeObjectURL(this.blobUrl);

            this.pendingFile = file; // giữ tạm, chưa upload
            this.mainFileName = file.name;

            // preview local bằng blob
            this.blobUrl = URL.createObjectURL(file);
            this.previewKind = file.type.includes('pdf') ? 'pdf' : (file.type.startsWith('image/') ? 'image' : 'other');
            this.previewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.blobUrl);

      }

      // ====== Submit → CreateDraft ======
      submitting = false;
      async submit() {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }
            this.submitting = true;

            try {
                  const form = this.form.value;

                  // 1) Tạo draft (header + mainFileId)
                  const draftBody = {
                        invoiceNumber: form.invoiceNumber!,
                        invoiceName: form.invoiceName!,
                        issueDate: form.issueDate!, // ISO string
                        paymentDate: form.paymentDate || null,

                        sellerName: form.sellerName || "",
                        sellerTaxCode: form.sellerTaxCode!,
                        sellerAddress: form.sellerAddress || null,

                        buyerName: form.buyerName || null,
                        buyerTaxCode: form.buyerTaxCode || null,
                        buyerAddress: form.buyerAddress || null,
                  };
                  const draftInvoice = await firstValueFrom(this.invoiceService.createDraft(draftBody));
                  const invoiceId = draftInvoice.id;

                  // 2) nếu có file → upload & replace-main
                  if(this.pendingFile) {
                        const up = await firstValueFrom(
                              this.fileService.uploadFile(this.pendingFile, 'Expense', 'Invoice', invoiceId, false)
                        ); // upload gắn entity id
                        const fileId = (up as any).id ?? (up as any).data?.id;
                        await firstValueFrom(
                              this.invoiceService.replaceMainFile(invoiceId, { newFileId: fileId })
                        );
                  }

                  // 3) Thêm thông tin hóa đơn
                  const lines = (form.invoiceLines ?? []) as AddInvoiceLineRequest[];
                  if(lines.length) {
                        const reqs = lines.map(x => ({
                              itemName: x.itemName,
                              unit: x.unit ?? null,
                              quantity: Number(x.quantity) || 0,
                              unitPrice: Number(x.unitPrice) || 0,
                              discountRate: x.discountRate ?? null,
                              discountAmount: x.discountAmount ?? null,
                              taxRatePercent: x.vatRatePercent ?? null,
                              whtTypeId: x.whtTypeId ?? null,
                        }));
                        await lastValueFrom(
                              forkJoin(reqs.map(r => this.invoiceService.addLine(invoiceId, r)))
                        );
                  }

                  this.router.navigate(["/expense/invoices"]);

                  alert("Tạo hóa đơn thành công");
            } catch(err) {
                  alert("Tạo hóa đơn thất bại");
            } finally {
                  this.submitting = false;
            }
      } 

      discountNotExceedGrossValidator = (group: AbstractControl): ValidationErrors | null => {
            const quantity = Number(group.get('quantity')?.value) || 0;
            const price = Number(group.get('unitPrice')?.value) || 0;
            const discount = Number(group.get('discountAmount')?.value) || 0;

            const gross = quantity * price;
            if(discount > gross) 
                  return  { discountTooHigh: true }
            
            return null;
      }

      cancel() {
            this.router.navigate(['/expense/invoices']);
      }

      ngOnDestroy(): void {
            if(this.blobUrl?.startsWith('blob:'))
                  URL.revokeObjectURL(this.blobUrl);
      }

      private recomputeTotals() {
            let g = 0, v = 0, t = 0;
            for(const ctrl of this.lines.controls) {
                  const gross = Number(ctrl.get('gross')?.value) || 0;
                  const vat = Number(ctrl.get('vatAmount')?.value) || 0;
                  const total = Number(ctrl.get('totalWithVat')?.value) || 0;
                  g += gross; v += vat; t += total;
            }
            this.totalGross = g;
            this.totalVat = v;
            this.totalWithVat = t;
      }
}