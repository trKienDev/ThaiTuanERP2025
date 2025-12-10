import { UserBriefAvatarDto } from "../../../../modules/account/models/user.model";

export interface NotificationDto {
      id: string;
      title: string;
      message: string;

      link?: string; /** Angular route string */
      linkType: NotificationLinkType; /** Loại link để UI tùy biến icon / điều hướng */
      targetId?: string | null; /** ID của đối tượng liên quan (BudgetPlanId, ExpenseId, RequestId ...) */
      type: NotificationType; /** Loại thông báo: Info / Warning / Task / Approval / System */

      createdAt: string;   
      isRead: boolean;

      senderId: string;
      sender: UserBriefAvatarDto;
}

export enum NotificationLinkType {
      None = 0,
      BudgetPlanReview = 1,
      BudgetPlanDetail = 2,
      ExpensePaymentDetail = 3,
      RequestDetail = 4,
      Dashboard = 5
}

export enum NotificationType {
      Info = 0,
      Warning = 1,
      Task = 2,
      Approval = 3,
      System = 4
}