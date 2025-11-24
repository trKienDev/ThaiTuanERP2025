import { ExpenseStepTemplatePayload } from "./expense-step-template.model";

export interface ExpenseWorkflowTemplateDto {
      id: string;
      name: string;
      documentType: string;
      version: number;
      isActive: boolean;
}

export interface ExpenseWorkflowTemplatePayload {
      name: string;
      version: number;
      steps: ExpenseStepTemplatePayload[];
}
