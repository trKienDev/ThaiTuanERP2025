import { inject, signal } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { ExpensePaymentDetailDto } from "../models/expense-payment.model";
import { ExpensePaymentService } from "../services/expense-payment.service";
import { ToastService } from "../../../shared/components/toast/toast.service";

export function usePaymentDetail() {
	const expensePaymentService = inject(ExpensePaymentService);

	// Reactive signals
	const paymentDetail = signal<ExpensePaymentDetailDto | null>(null);
	const isLoading = signal(false);
	const error = signal<string | null>(null);
	const toastService = inject(ToastService);

	async function load(paymentId: string) {
		try {
			isLoading.set(true);
			error.set(null);

			const detail = await firstValueFrom(expensePaymentService.getDetailById(paymentId));
			paymentDetail.set(detail);
		} catch (err: any) {
			console.error('Failed to load payment detail', err);
			error.set('Không thể tải dữ liệu thanh toán');
                  toastService.errorRich("Không thể tải dữ liệu thanh toán");
		} finally {
			isLoading.set(false);
		}
	}

	// Optional: refresh function
	async function refresh() {
		const current = paymentDetail();
		if (current?.id) await load(current.id);
	}

	return { paymentDetail, isLoading, error, load, refresh };
}