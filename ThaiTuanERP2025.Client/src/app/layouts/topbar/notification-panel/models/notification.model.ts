export interface NotificationDto {
      id: string;
      title: string;
      message: string;
      link?: string;
      createdAt: Date;
      isRead: boolean
}