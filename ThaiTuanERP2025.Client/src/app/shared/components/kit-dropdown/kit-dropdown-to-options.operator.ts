import { map, Observable } from "rxjs";
import { mapToDropdownOptions, OptionsAdapterConfig } from "./kit-dropdown-options.adapter";
import { KitDropdownOption } from "./kit-dropdown.component";

export function toDropdownOptions<TModel, TId = any>(
      cfg: OptionsAdapterConfig<TModel, TId>
) {
      return (source$: Observable<TModel[]>): Observable<KitDropdownOption<TId>[]> => 
            source$.pipe(map(list => mapToDropdownOptions(list, cfg)));
}