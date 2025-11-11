import { KitDropdownOption } from "./kit-dropdown.component";

type KeyOf<T> = Extract<keyof T, string>;

export type OptionsAdapterConfig<T> = {
      id: KeyOf<T> | ((item: T) => string);
      label: KeyOf<T> | ((item: T) => string);
      imgUrl?: KeyOf<T> | ((item: T) => string | undefined);
}

export function mapToDropdownOptions<T>( list: T[], cfg: OptionsAdapterConfig<T> ): KitDropdownOption[] {
      const getId = (item: T) => typeof cfg.id === 'function' ? cfg.id(item) : String(item[cfg.id]);
      const getLabel = (item: T) => typeof cfg.label === 'function' ? cfg.label(item) : String(item[cfg.label] ?? '');

      const getImg = (item: T) => typeof cfg.imgUrl === 'function'
            ? cfg.imgUrl(item)
            : (cfg.imgUrl ? String(item[cfg.imgUrl] ?? '') : undefined);

      return (list ?? []).map(item => {
            const opt: KitDropdownOption = {
                  id: getId(item),
                  label: getLabel(item),
            };
            const url = cfg.imgUrl ? getImg(item) : undefined;
            if (url) (opt as any).imgUrl = url;
            return opt;
      });
}