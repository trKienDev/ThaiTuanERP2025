import { ExpenseStepTemplateDto, ExpenseStepTemplatePayload } from "./expense-step-template.model";

export interface ExpenseWorkflowTemplateDto {
      id: string;
      name: string;
      version: number;
      isActive: boolean;
      steps: ExpenseStepTemplateDto[];
}

export interface ExpenseWorkflowTemplatePayload {
      name: string;
      version: number;
      steps: ExpenseStepTemplatePayload[];
}

export function mapExpenseWorkflowTemplateDtoToPayload(dto: ExpenseWorkflowTemplateDto): ExpenseWorkflowTemplatePayload {
      return {
            name: dto.name,
            version: dto.version,
            steps: dto.steps.map(step => ({
                  name: step.name,
                  order: step.order,
                  flowType: step.flowType,
                  slaHours: step.slaHours,
                  approveMode: step.approverMode,
                  approverIds: step.approverIds ?? [],
                  resolverKey: step.resolverKey ?? null,
                  resolverParams: step.resolverParams ?? null
            }))
      };
}
