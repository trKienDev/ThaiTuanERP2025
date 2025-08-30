<h1 style="color: blue">ApprovalResolverService</h1>

# Vai trò của **ApproverResolverService**
- Trong **Approval Workflow**, mỗi bước duyệt <span style="color: pink">(StepDifintion)</span> chỉ lưu **kiểu chọn người duyệt** (*ResolverType*) và tham **tham số cấu hình** (*ResolverParamsJson*).
- Khi **submit phiếu**, hệ thống cần biết **chính xác user nào** sẽ được gán để duyệt trong <span style="color: pink">(StepInstance)</span> 
👉 **ApprovalResolverService** chính là **bộ chuyển đổi** từ **cấu hình step --> danh sách userId cụ thể** sẽ tham gia duyệt

# Nhiệm vụ chi tiết
1. Nhận vào
- **StepDifintion** (chứa **ResolverType**, **ParamJson**)
- **Context** (document, requester, selectedApproverId nếu có).

2. Dựa trên **ResolverType**:
- **FixedUser** trả về đúng 1 user đã config.
- **UserPickFromList** lấy user do requester chọn từ danh sách cho phép.
- **UserList** trả về toàn bộ danh sách user được config.

3. Xuất ra: danh sách **Guid** (candidates) để lưu vào **StepInstance.CandidatesJson**

# Lý do cần có
- **Tách trách nhiệm**: Submit handler chỉ lo tạo *FlowInstance*/*StepInstance*, không phải chứa if/else phức tạp.
- **Dễ mở rộng**: sau này muốn thêm các lựa chọn khác (theo role, theo manager, theo phòng ban, ...) chỉ cần bổ sung *Resolver*, không phải sửa handler.
- **Dễ tái sử dụng**: có thể dùng khi preview flow (xem trước ai duyệt) hoặc trong báo cáo.

# Tóm lại
**ApproverResolveService** là chỗ tập trung toàn bộ logic "Ai sẽ duyệt?", cho từng step, để thống Approval gọn gàng, dễ mở rộng & bảo trì.

# Sơ đồ
## flowchart LR
**A**[ApprovalStepDefinition<br/>ResolverType + ParamsJson] --> B(ApproverResolverService)

**subgraph B** [ApproverResolverService]
- B1{ResolverType?}
- B1 -->|FixedUser| C1[Trả về 1 userId]
- B1 -->|UserPickFromList| C2[Kiểm tra SelectedApproverId<br/>nằm trong danh sách cho phép<br>→ trả về 1 userId]
- B1 -->|UserList| C3[Trả về nhiều userIds<br/>(candidates)]
end

C1 --> D[Danh sách userIds]
C2 --> D
C3 --> D

D --> E[ApprovalStepInstance.CandidatesJson]

## Giải thích
1. **StepDefinition** chỉ lưu cấu hình (ví dụ: ResolverType = UserList, ParamsJson = {userIds: [u1,u2,u3]}).
2. Khi Submit, handler gọi **ApproverResolverService**.
3. Service đọc *ResolverType*, parse *ParamsJson*, áp dụng logic → trả về danh sách *UserIds*.
4. Danh sách đó được gắn vào **StepInstance.CandidatesJson** → dùng cho runtime (ai thấy phiếu, ai có quyền Approve/Reject).