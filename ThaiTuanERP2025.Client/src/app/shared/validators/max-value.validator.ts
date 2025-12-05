import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function maxRemainingValidator(max: number): ValidatorFn {
      return (control: AbstractControl): ValidationErrors | null => {
            if (control.value == null) return null;

            return control.value > max
                  ? { maxRemaining: { requiredMax: max, actual: control.value } }
                  : null;
      };
}