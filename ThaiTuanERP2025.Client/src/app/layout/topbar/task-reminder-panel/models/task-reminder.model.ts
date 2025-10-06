export interface TaskReminderDto {
      id: string;
      title: string;
      message: string;
      dueAt: string;
      workflowInstanceId: string;
      stepInstanceId: string;
      documentId?: string;  // tuỳ chọn, có thể dùng để điều hướng
      documentType?: string; // tuỳ chọn, có thể dùng để điều hướng
}

