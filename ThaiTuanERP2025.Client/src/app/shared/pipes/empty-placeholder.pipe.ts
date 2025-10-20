import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'emptyPlaceholder', standalone: true })
export class EmptyPlaceholderPipe implements PipeTransform {
      transform(value: any, placeholder: string = '----'): { text: string; isPlaceholder: boolean } {
            const isEmpty = value === null || value === undefined || value === '';
            return {
                  text: isEmpty ? placeholder : value,
                  isPlaceholder: isEmpty
            };
      }
}
