# Entity & Schema Definition
- ApprovalAction *(1)*
- ApprovalFlowDefinition *(1)*
- ApprovalFlowInstance *(1)*
- ApprovalStepDefinition *(1)*
- ApprovalStepInstance *(1)*

# Controllers
- Reject *(10)*
- GetFlowForDocument *(10)*
- GetMyPending *(10)*
- Approve *(10)*
- Comment *(10)*


# CQRS
- SubmitApprovalCommand *(3)* : submit chứng từ
- ApproveStepCommand *(4)* : đóng step & mở step tiếp theo
- RejectStepCommand *(5)* : 
- CommentOnStepCommand *(6)*
- GetStepActionsQuery *(7)*
- GetFlowInstanceByDocumentQuery *(8)*
- GetMyPendingApprovalQuery *(9)*

# Auth
- CanApproveStepCommand *(10)*
- CanCommenttOnStepCommand *(10)*

# Services
- ApproverResolverService *(2)*

# Auth
- CanApproveStepRequirement *(10)*
- CanCommentStepRequirement *(10)*

# Repository
- ApprovalFlowDefinitionRepository *(3)*
- ApprovalFlowInstanceRepository *(3)*
- ApprovalActionRepsitory *(4)*