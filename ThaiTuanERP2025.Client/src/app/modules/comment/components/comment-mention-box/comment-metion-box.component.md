# CommentMentionBoxComponent Documentation

## 1. Overview

`CommentMentionBoxComponent` hỗ trợ tính năng mention user trong comment editor. Khi người dùng gõ ký tự `@`, component sẽ mở popup gợi ý user, cho phép filter theo keyword, chọn user và chèn vào nội dung input.

## 2. Features Summary

| Feature | Mô tả |
|--------|-------|
| Detect mention | Nhận diện khi người dùng gõ `@keyword` |
| Suggestion popup | Hiển thị danh sách user phù hợp |
| Filter by keyword | Lọc theo label (không phân biệt hoa/thường) |
| Remove mentioned users | Loại bỏ user đã được mention trước đó |
| Commit mention | Insert dạng `@Full Name ` |
| Resolve raw mention | Xử lý khi người dùng gõ thêm chữ sau mention |
| Keyboard navigation | ArrowUp / ArrowDown / Enter |
| Real-time sync | Đồng bộ mentionedSet khi input thay đổi |

## 3. Mention Detection Logic

Component dùng regex để detect mention đang nhập:

```
/@([A-Za-zÀ-ỹ0-9 ._-]*)$/
```

Nếu match → người dùng đang gõ mention → mở popup.

## 4. Mention Commit Recognition

Mention được xem là hoàn tất (commit) khi có dạng:

```
@Full Name␣
```

Regex nhận diện:

```
/@([A-Za-zÀ-ỹ0-9 ._-]+)\s/g
```

Sau khi match, phần label được đưa vào `mentionedSet`.

## 5. Why resolveMentionLabel is needed

Khi user đã chọn mention:

```
@Phó giám đốc IT␣
```

Nhưng nếu gõ thêm:

```
@Phó giám đốc IT nhớ gặp
```

Regex sẽ match `"Phó giám đốc IT nhớ"`, nhưng đây không phải label thật.

Hàm `resolveMentionLabel()` sẽ tìm label thật trong danh sách user:

- Lấy tất cả user có label là prefix của raw
- Ưu tiên label dài nhất

Ví dụ raw: `"Phó giám đốc IT nhớ"` → label: `"Phó giám đốc IT"`.

## 6. Filtering Logic

Hai bước:

1. Loại bỏ user đã được mention:

```
filterOutMentioned(users)
```

2. Lọc tiếp theo keyword:

```
u.label.toLowerCase().includes(currentKeyword)
```

## 7. Keyboard Navigation

| Key | Action |
|------|--------|
| ArrowDown | Di chuyển highlight xuống |
| ArrowUp | Di chuyển highlight lên |
| Enter | Select mention đang highlight |

## 8. Insert Mention

Mention được chèn bằng regex:

```
current.replace(/@[A-Za-zÀ-ỹ0-9 ._-]*$/, `@${user.label} `)
```

Ví dụ:

Input: `@Ki` → Select “Kiên” → Output: `@Kiên `.

## 9. State Objects

| State | Chức năng |
|--------|-----------|
| mentionedSet | Các user đã mention |
| allUsers | Danh sách user gốc để resolve label |
| currentUsers | Danh sách user hiển thị |
| currentKeyword | Keyword đang nhập |
| activeIndex | Index highlight |
| showMentionBox | Boolean bật/tắt popup |

## 10. Component Structure

```
comment-mention-box/
│
├── comment-mention-box.component.ts
├── comment-mention-box.component.html
├── comment-mention-box.component.scss
└── README.md   ← file tài liệu này
```

## 11. Summary

Component xử lý:

- Detect mention  
- Filter logic  
- Resolve label  
- Keyboard navigation  
- Maintain mentionedSet  
- Sync input text  

Phiên bản refactor rõ ràng, dễ maintain và dễ mở rộng.

