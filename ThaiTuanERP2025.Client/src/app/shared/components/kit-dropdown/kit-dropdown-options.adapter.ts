import { KitDropdownOption } from "./kit-dropdown.component";

type KeyOf<T> = Extract<keyof T, string>;

export type OptionsAdapterConfig<TModel, TId = any> = {
      id: KeyOf<TModel> | ((item: TModel) => TId);
      label: KeyOf<TModel> | ((item: TModel) => string);
      imgUrl?: KeyOf<TModel> | ((item: TModel) => string | undefined);
}

export function mapToDropdownOptions<TModel, TId = any>( list: TModel[], cfg: OptionsAdapterConfig<TModel, TId> ): KitDropdownOption<TId>[] {
      const getId = (item: TModel): TId => 
            typeof cfg.id === 'function'  
                  ? cfg.id(item) 
                  : ((item as any)[cfg.id] as TId);
      
      const getLabel = (item: TModel): string => 
                  typeof cfg.label === 'function' 
                        ? cfg.label(item) 
                        : String((item as any)[cfg.label] ?? '');

      const getImg = (item: TModel) => 
            typeof cfg.imgUrl === 'function'
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