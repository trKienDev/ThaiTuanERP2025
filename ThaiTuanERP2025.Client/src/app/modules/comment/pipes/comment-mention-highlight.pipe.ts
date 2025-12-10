import { Pipe, PipeTransform } from "@angular/core";
import { CommentMentionDto } from "../models/comment.model";

@Pipe({
      name: 'commentMentionHighlight',
      standalone: true
})
export class MentionHighlightPipe implements PipeTransform {
      transform(content: string, mentions: CommentMentionDto[]): string {
            let html = content;

            for (const m of mentions) {
                  const escapedLabel = m.fullName.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
                  const regex = new RegExp(`@${escapedLabel}`, 'g');

                  html = html.replace(
                        regex, 
                        `<span class="user-mention" data-id="${m.id}">@${m.fullName}</span>`
                  );
            }

            return html;
      }
}