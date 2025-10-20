export interface NotificationPayload {
      id: string;
      title: string;
      message: string;
      link?: string;   
      createdAt: string | Date;     
      isRead: boolean;
}

export interface NotificationDto extends NotificationPayload {}