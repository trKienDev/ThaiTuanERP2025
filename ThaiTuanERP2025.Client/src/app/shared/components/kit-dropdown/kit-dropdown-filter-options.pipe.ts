import { Pipe, PipeTransform } from '@angular/core';

export interface DropdownOptionLike {
  id: string;
  label: string;
  imgUrl?: string;
}

@Pipe({
  name: 'dropdownFilterOptions',
  standalone: true,
  pure: true,
})
export class DropdownFilterOptionsPipe implements PipeTransform {
  transform(
    options: DropdownOptionLike[] | null | undefined,
    filterText: string | null | undefined,
    caseSensitive = false
  ): DropdownOptionLike[] {
    const list = options ?? [];
    const q = (filterText ?? '').trim();
    if (!q) return list;

    if (caseSensitive) {
      return list.filter(o => (o.label ?? '').includes(q));
    }
    const nq = q.toLocaleLowerCase();
    return list.filter(o => (o.label ?? '').toLocaleLowerCase().includes(nq));
  }
}
