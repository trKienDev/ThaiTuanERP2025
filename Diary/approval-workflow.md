# Entity & Schema Definition
- ApprovalAction *(1)*
- ApprovalFlowDefinition *(1)*
- ApprovalFlowInstance *(1)*
- ApprovalStepDefinition *(1)*
- ApprovalStepInstance *(1)*

# CQRS
- SubmitApprovalCommand *(3)*
- ApproveStepCommand *(4)*
- RejectStepCommand *(5)*
- CommentOnStepCommand *(6)*

# Services
- ApproverResolverService *(2)*



# Repository
- ApprovalFlowDefinitionRepository *(3)*
- ApprovalFlowInstanceRepository *(3)*
- ApprovalActionRepsitory *(4)*