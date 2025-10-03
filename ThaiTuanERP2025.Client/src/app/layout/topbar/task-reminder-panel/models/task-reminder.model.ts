export interface TaskReminderDto {
      id: string;
      title: string;
      message: string;
      dueAt: string;
      workflowInstanceId: string;
      stepInstanceId: string;
}

