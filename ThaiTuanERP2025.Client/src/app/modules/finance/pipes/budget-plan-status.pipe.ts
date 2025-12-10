import { Pipe, PipeTransform } from "@angular/core";

export type BudgetPlanStatusView = { text: string; class: string };

@Pipe({ name: 'budgetPlanStatus', standalone: true })
export class BudgetPlanStatusPipe implements PipeTransform {
      transform(status: number | null | undefined): BudgetPlanStatusView {
            switch (status) {
                  case 0: return { text: 'Mới tạo', class: 'draft' };
                  case 1: return { text: 'Đã xem xét', class: 'reviewed' };
                  case 2: return { text: 'Đã duyệt', class: 'approved' };
                  case 4: return { text: 'Bị từ chối', class: 'rejected' };
                  case 5: return { text: 'Đã hủy', class: 'cancelled' };
                  default: return { text: 'Không xác định', class: 'unknown' }
            }
      }
}
