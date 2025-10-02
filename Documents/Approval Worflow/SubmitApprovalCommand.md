```csharp
public async Task<Guid> Handle(SubmitApprovalCommand request, CancellationToken ct)
{
      // 1) Lấy FlowDefinition active mới nhất
      var def = await _uow.ApprovalFlowDefinitions
      .GetLatestActiveByDocumentTypeAsync(request.DocumentType, ct);

      if (def is null)
      throw new InvalidOperationException($"No active approval flow for {request.DocumentType}");

      // 2) Tạo FlowInstance
      var instance = new ApprovalFlowInstance
      {
      Id = Guid.NewGuid(),
      FlowDefinitionId = def.Id,
      DefinitionVersion = def.Version,
      DocumentType = request.DocumentType,
      DocumentId = request.DocumentId,
      Status = ApprovalFlowStatus.InProgress,
      StartedAt = DateTime.UtcNow,
      CreatedByUserId = _currentUser.UserId,
      CreatedDate = DateTime.UtcNow
      };

      // 3) Resolve & tạo StepInstances
      foreach (var stepDef in def.Steps) // đã được order ở repo
      {
      var candidates = _resolver.Resolve(stepDef, request.SelectedApproverId);
      if (candidates is null || !candidates.Any())
            throw new InvalidOperationException($"Step '{stepDef.Name}' has no candidates.");

      instance.Steps.Add(new ApprovalStepInstance
      {
            Id = Guid.NewGuid(),
            FlowInstanceId = instance.Id,
            StepDefinitionId = stepDef.Id,
            Name = stepDef.Name,
            OrderIndex = stepDef.OrderIndex,
            Status = ApprovalStepStatus.Pending,
            CandidatesJson = JsonSerializer.Serialize(candidates),
            RequiredCount = stepDef.RequiredCount,
            ApprovedCount = 0,
            CreatedByUserId = _currentUser.UserId,
            CreatedDate = DateTime.UtcNow
      });
      }

      // 4) Kích hoạt bước đầu tiên
      var first = instance.Steps.OrderBy(s => s.OrderIndex).First();
      first.Status = ApprovalStepStatus.InProgress;
      first.StartedAt = DateTime.UtcNow;
      // nếu bạn cần Deadline theo SLA: cần SlaHours từ StepDefinition → clone sang StepInstance khi tạo
      // (nếu đã có, set first.DeadlineAt = StartedAt + SlaHours)

      // 5) Lưu
      await _uow.ApprovalFlowInstances.AddAsync(instance, ct);
      await _uow.SaveChangesAsync(ct);

      return instance.Id;
}
```

# Giải thích
## 1. Lấy *FlowDefinition* active mới nhất
* **Versioning & bất biến theo yêu cầu** : bạn đã xác nhận “thay đổi flow chỉ áp dụng cho request mới” → Vì vậy, mỗi lần submit phải khóa vào bản thiết kế (definition) mới nhất đang Active để:
- Gắn *DefinitionVersion* cho *FlowInstance* → về sau dù admin có sửa flow, instance đang chạy không bị ảnh hưởng.
- Bảo đảm *tính tái lập (audit)*: khi xem lại lịch sử, biết chính xác request này chạy theo version nào.
* **Tránh config lỗi/treo** : nếu không có flow active → fail sớm, tránh sinh instance mồ côi.

## 2. Resolve approvers (từ StepDefinition → danh sách user cụ thể)
- **Definition chỉ là cấu hình tĩnh** : *ResolverType* + *ResolverParamsJson* (FixedUser/UserPickFromList/UserList). Nó chưa cho biết user nào sẽ duyệt ở thời điểm thực thi.
- **Runtime cần người thật để gán quyền**
      - Case A (FixedUser): 1 user cố định.
      - Case B (UserPickFromList): người lập phiếu chọn 1 trong danh sách allowed → phải kiểm tra & chốt lại **1 user duy nhất**.
      - Case C (UserList): n người thấy, nhưng *RequiredCount = 1* → ai duyệt trước thì khóa bước.
- **Kết quả resolve** chính là Candidates để lưu vào *StepInstance.CandidatesJson*. Không làm bước này ngay khi submit thì:
      - UI không biết ai được phép duyệt.
      - Notify không biết gửi cho ai.
      - Khi vào bước, lại phải “resolve động” → dễ phát sinh sai lệch nếu cấu hình thay đổi.

## 3. Tạo StepInstances ngay khi submit
- **Đóng băng cấu hình theo version** : bạn clone thông tin cần thiết từ *StepDefinition* về *StepInstance* (Name, OrderIndex, RequiredCount…). Điều này:
      - Giữ được **lịch sử chính xác** (kể cả khi admin đổi tên step sau này).
      - Không phụ thuộc file cấu hình sau submit.
- **Khởi động quy trình** : set bước đầu tiên *InProgress*, tính *StartedAt/DeadlineAt* (nếu dùng SLA), và có thể phát **notify ngay** cho candidates của bước 1.
- **Dễ điều hướng & kiểm soát** : có đầy đủ “hàng chờ” các bước tiếp theo (Pending) để handler Approve chỉ việc: Đóng step hiện tại → Mở step kế tiếp (đã có sẵn, không phải “tự tính lại”).
- **An toàn với thay đổi flow**: mọi StepInstance đã sinh là **bất biến** theo definition version lúc submit, đúng nguyên tắc “request cũ không bị ảnh hưởng”.

