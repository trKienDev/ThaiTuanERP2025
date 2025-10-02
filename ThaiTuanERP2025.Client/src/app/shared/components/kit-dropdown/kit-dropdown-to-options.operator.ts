import { map, Observable } from "rxjs";
import { mapToDropdownOptions, OptionsAdapterConfig } from "./kit-dropdown-options.adapter";
import { KitDropdownOption } from "./kit-dropdown.component";

export function toDropdownOptions<T>(
      cfg: OptionsAdapterConfig<T>
) {
      return (source$: Observable<T[]>): Observable<KitDropdownOption[]> => 
            source$.pipe(map(list => mapToDropdownOptions(list, cfg)));
}