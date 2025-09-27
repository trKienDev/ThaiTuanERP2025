import { FormGroup } from "@angular/forms";

export function logFormErrors(form: FormGroup, parentKey: string = ''): void {
      Object.keys(form.controls).forEach(key => {
            const control = form.get(key);
            const fullKey = parentKey ? `${parentKey}.${key}` : key;

            if(control instanceof FormGroup) {
                  logFormErrors(control, fullKey);
            } else {
                  if(control && control.invalid) {
                        console.warn(`❌ Control "${fullKey}" invalid, errors: `, control.errors );
                  }
            }
      })
}