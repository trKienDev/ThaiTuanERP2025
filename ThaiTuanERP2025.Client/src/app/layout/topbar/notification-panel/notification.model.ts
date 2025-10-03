export interface NotificationPayload {
      userId?: string;
      title?: string;
      message?: string;
      createdAt?: string | Date;
      icon?: string;     
      link?: string;      
      unread?: boolean;
}