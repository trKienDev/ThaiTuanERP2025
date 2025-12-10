import { ValidatorFn, AbstractControl, ValidationErrors } from "@angular/forms";

/**
 * Kiểm tra rằng EndDate >= StartDate
 */
export function endDateAfterStartDateValidator(): ValidatorFn {
      return (group: AbstractControl): ValidationErrors | null => {
            const startDate = group.get('startDate')?.value;
            const endDate = group.get('endDate')?.value;

            if (!startDate || !endDate) return null;

            const start = new Date(startDate);
            const end = new Date(endDate);

            return end >= start ? null : { endBeforeStart: true };
      };
}