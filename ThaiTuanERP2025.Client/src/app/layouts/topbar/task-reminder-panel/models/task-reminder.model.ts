export interface TaskReminderDto {
      id: string;
      subject: string;
      message: string;
      dueAt: string;
      slaHours: number;
      isResolved: boolean;
      linkUrl?: string;
}

