import { Injectable, signal } from "@angular/core";
import { CommentDetailDto } from "../models/comment.model";

@Injectable({ providedIn: 'root' })
export class CommentStateService {
      private expandMap = signal<Record<string, boolean>>({});

      /** Component đọc trạng thái này */
      isExpanded(id: string): boolean {
            return this.expandMap()[id] === true;
      }

      /** Mở 1 comment */
      expand(id: string): void {
            this.expandMap.update(map => ({ ...map, [id]: true }));
      }

      /** Đóng 1 comment */
      collapse(id: string): void {
            this.expandMap.update(map => ({ ...map, [id]: false }));
      }

      /** Mở nguyên cây (đệ quy) */
      expandRecursive(node: CommentDetailDto): void {
            this.expand(node.id);
            (node.replies ?? []).forEach(child => this.expandRecursive(child));
      }

      /** Đóng nguyên cây (đệ quy) */
      collapseRecursive(node: CommentDetailDto): void {
            this.collapse(node.id);
            (node.replies ?? []).forEach(child => this.collapseRecursive(child));
      }
}