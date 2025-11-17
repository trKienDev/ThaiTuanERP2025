import { inject, signal } from "@angular/core";
import { OutgoingPaymentApiService } from "../services/outgoing-payment.service";
import { OutgoingPaymentDetailDto } from "../models/outgoing-payment.model";
import { ToastService } from "../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { firstValueFrom } from "rxjs";

export function useOutgoingPaymentDetail() {
      const outgoingPaymentApi = inject(OutgoingPaymentApiService);

      const outgoingPaymentDetail = signal<OutgoingPaymentDetailDto | null>(null);
      const isLoading = signal(false);
      const error = signal<string | null>(null);
      const toastService = inject(ToastService);

      async function load(outgoingPaymentId: string) {
            try {
                  isLoading.set(true);
                  error.set(null);
            
                  const detail = await firstValueFrom(outgoingPaymentApi.getDetailById(outgoingPaymentId));
                  outgoingPaymentDetail.set(detail);
            } catch (err) {
                  console.error('Failed to load payment detail', err);
			error.set('Không thể tải dữ liệu khoản chi');
                  toastService.errorRich("Không thể tải dữ liệu khoản chi");
            } finally {
                  isLoading.set(false);
            }
      }

      async function refresh() {
            const currentDetail = outgoingPaymentDetail();
            if (currentDetail?.id) await load(currentDetail.id);
      }

      return { outgoingPaymentDetail, isLoading, error, load, refresh };
}